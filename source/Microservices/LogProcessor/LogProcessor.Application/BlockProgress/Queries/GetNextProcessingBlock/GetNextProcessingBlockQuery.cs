using BaseApplication.Exceptions;
using BlockChainQueryHelper.Interfaces;
using BlockChainWeb3ProviderHelper.Interfaces;
using CacheManager.Interfaces;
using LogProcessor.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nethereum.Util;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace LogProcessor.Application.BlockProgress.Queries.GetNextProcessingBlock;

public record GetNextProcessingBlockQuery([property: Required] Nethereum.Signer.Chain Chain) : IRequest<BigInteger[]>;

public class Handler(IApplicationDbContext context, IDistributedLockService<BigInteger[]> distributedLockService, IBlockService blockService, IWeb3ProviderService web3ProviderService) : IRequestHandler<GetNextProcessingBlockQuery, BigInteger[]>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IDistributedLockService<BigInteger[]> _distributedLockService = distributedLockService;
    private readonly IBlockService _blockService = blockService;
    private readonly IWeb3ProviderService _web3ProviderService = web3ProviderService;
    private readonly CustomWaitStrategy _waitStrategy = new();

    public async Task<BigInteger[]> Handle(GetNextProcessingBlockQuery request, CancellationToken cancellationToken)
    {
        return await _distributedLockService.RunWithLockAsync(async () => await GetNextProcessingBlock(request.Chain, cancellationToken), 
            key: $"BlockProcessor_NextProcessingBlock_{request.Chain}",
            expiryInSecond: 30, waitInSecond: 3 * 60, retryInSecond: 5, cancellationToken: cancellationToken);
    }

    private async Task<BigInteger[]> GetNextProcessingBlock(Nethereum.Signer.Chain chain, CancellationToken cancellationToken)
    {
        long fromBlockNumber, toBlockNumber;

        var rpcUrls = await _context.RpcUrls.Where(exp => exp.Chain == chain).Select(s => new { s.Uri, s.BlockOfConfirmation, s.MaxNumberOfBlocksPerProcess }).ToListAsync(cancellationToken);
        if (rpcUrls.Count == 0) throw new NotFoundException("No RPC Urls found for the chain");

        var rpcUrl = rpcUrls[0].Uri.ToString();
        var blockOfConfirmation = rpcUrls[0].BlockOfConfirmation;
        var numberOfBlocksPerRequest = rpcUrls[0].MaxNumberOfBlocksPerProcess;

        var web3 = _web3ProviderService.CreateWeb3(rpcUrl);
        var lastBlockOfBlockChain = await _blockService.GetLastBlockAsync(web3, cancellationToken);

        if (!await _context.BlockProgresses.AnyAsync(exp => exp.Chain == chain, cancellationToken)) 
        {
            fromBlockNumber = (long)lastBlockOfBlockChain - blockOfConfirmation;
            toBlockNumber = (long)lastBlockOfBlockChain;
        }
        else
        {
            var maxBlockNumber = await _context.BlockProgresses.Where(exp => exp.Chain == chain).Select(s => s.BlockNumber).MaxAsync(cancellationToken);
            fromBlockNumber = maxBlockNumber + 1;
            toBlockNumber = CalculateToBlockNumber(fromBlockNumber, numberOfBlocksPerRequest, (long)lastBlockOfBlockChain, blockOfConfirmation);

            uint attemptCount = 0;
            while (toBlockNumber <= fromBlockNumber)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _waitStrategy.ApplyAsync(attemptCount).ConfigureAwait(false);
                attemptCount++;
                lastBlockOfBlockChain = await _blockService.GetLastBlockAsync(web3, cancellationToken);
                toBlockNumber = CalculateToBlockNumber(fromBlockNumber, numberOfBlocksPerRequest, (long)lastBlockOfBlockChain, blockOfConfirmation);
            }
        }

        BigInteger[] result = [];
        for(long blockNumber = fromBlockNumber; blockNumber <= toBlockNumber; blockNumber++)
        {
            var blockProgress = new Domain.Entities.BlockProgress
            {
                BlockNumber = blockNumber,
                Chain = chain,
                Status = Domain.Entities.BlockProgress.BlockProgressStatus.Processing
            };
            await _context.BlockProgresses.AddAsync(blockProgress, cancellationToken);

            result = [.. result, new BigInteger(blockNumber)];
        }

        await _context.SaveChangesAsync(cancellationToken);

        return result;
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

    private static long CalculateToBlockNumber(long fromBlockNumber, int numberOfBlocksPerRequest, long lastBlockOfBlockChain, int blockOfConfirmation)
    {
        return fromBlockNumber + numberOfBlocksPerRequest - 1 <= lastBlockOfBlockChain - blockOfConfirmation
                    ? fromBlockNumber + numberOfBlocksPerRequest - 1
                    : lastBlockOfBlockChain - blockOfConfirmation;
    }
}
