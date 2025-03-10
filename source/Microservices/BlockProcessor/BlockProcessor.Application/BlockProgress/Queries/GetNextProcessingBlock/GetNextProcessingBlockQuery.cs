using BaseApplication.Exceptions;
using BlockChainQueryHelper.Interfaces;
using BlockChainWeb3ProviderHelper.Interfaces;
using BlockProcessor.Application.Interfaces;
using CacheManager.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nethereum.Util;
using System.ComponentModel.DataAnnotations;

namespace BlockProcessor.Application.BlockProgress.Queries.GetNextProcessingBlock;

public record GetNextProcessingBlockQuery([property: Required] Nethereum.Signer.Chain Chain) : IRequest<long>;

public class Handler(IApplicationDbContext context, IDistributedLockService<long> distributedLockService, IBlockService blockService, IWeb3ProviderService web3ProviderService) : IRequestHandler<GetNextProcessingBlockQuery, long>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IDistributedLockService<long> _distributedLockService = distributedLockService;
    private readonly IBlockService _blockService = blockService;
    private readonly IWeb3ProviderService _web3ProviderService = web3ProviderService;
    private readonly CustomWaitStrategy _waitStrategy = new();

    public async Task<long> Handle(GetNextProcessingBlockQuery request, CancellationToken cancellationToken)
    {
        return await _distributedLockService.RunWithLockAsync(async () => await GetNextProcessingBlock(request.Chain, cancellationToken), $"BlockProcessor_NextProcessingBlock_{request.Chain}", cancellationToken: cancellationToken);
    }

    private async Task<long> GetNextProcessingBlock(Nethereum.Signer.Chain chain, CancellationToken cancellationToken)
    {
        long nextBlockNumber;

        var rpcUrls = await _context.RpcUrls.Where(exp => exp.Chain == chain).Select(s => new { s.Uri, s.BlockOfConfirmation }).ToListAsync(cancellationToken);
        if (rpcUrls.Count == 0) throw new NotFoundException("No RPC Urls found for the chain");

        var rpcUrl = rpcUrls[0].Uri.ToString();
        var blockOfConfirmation = rpcUrls[0].BlockOfConfirmation;

        var web3 = _web3ProviderService.CreateWeb3(chain, rpcUrl);
        var lastBlockOfBlockChain = await _blockService.GetLastBlockAsync(web3, cancellationToken);

        if (!await _context.BlockProgresses.AnyAsync(exp => exp.Chain == chain, cancellationToken)) 
        {
            nextBlockNumber = (long)lastBlockOfBlockChain - blockOfConfirmation;
            var processedBlock = new Domain.Entities.BlockProgress
            {
                BlockNumber = nextBlockNumber,
                Chain = chain,
                Status = Domain.Entities.BlockProgress.BlockProgressStatus.Processing
            };
            await _context.BlockProgresses.AddAsync(processedBlock, cancellationToken);
        }
        else
        {
            var maxBlockNumber = await _context.BlockProgresses.Where(exp => exp.Chain == chain).Select(s => s.BlockNumber).MaxAsync(cancellationToken);
            nextBlockNumber = maxBlockNumber + 1;

            uint attemptCount = 0;
            while (lastBlockOfBlockChain - blockOfConfirmation < nextBlockNumber)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _waitStrategy.ApplyAsync(attemptCount).ConfigureAwait(false);
                attemptCount++;
                lastBlockOfBlockChain = await _blockService.GetLastBlockAsync(web3, cancellationToken);
            }

            var processingBlock = new Domain.Entities.BlockProgress
            {
                BlockNumber = nextBlockNumber,
                Chain = chain,
                Status = Domain.Entities.BlockProgress.BlockProgressStatus.Processing
            };
            await _context.BlockProgresses.AddAsync(processingBlock, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return nextBlockNumber;
    }

    private sealed class CustomWaitStrategy : IWaitStrategy
    {
        private static readonly int[] WaitIntervals = [1000, 5000, 10000, 20000, 30000, 40000, 50000, 60000];

        public Task ApplyAsync(uint retryCount)
        {
            var intervalMs = retryCount >= WaitIntervals.Length ? WaitIntervals[^1] : WaitIntervals[retryCount];

            return Task.Delay(intervalMs);
        }
    }
}
