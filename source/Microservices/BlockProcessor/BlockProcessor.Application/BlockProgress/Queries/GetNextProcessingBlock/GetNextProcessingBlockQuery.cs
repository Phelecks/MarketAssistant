using BaseApplication.Exceptions;
using BlockChainQueryHelper.Interfaces;
using BlockChainWeb3ProviderHelper.Interfaces;
using BlockProcessor.Application.Interfaces;
using CacheManager.Interfaces;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlockProcessor.Application.BlockProgress.Queries.GetNextProcessingBlock;

public record GetNextProcessingBlockQuery([property: Required] Nethereum.Signer.Chain Chain) : IRequest<long>;

public class Handler(IApplicationDbContext context, IDistributedLockService<long> distributedLockService, IBlockService blockService, IWeb3ProviderService web3ProviderService) : IRequestHandler<GetNextProcessingBlockQuery, long>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IDistributedLockService<long> _distributedLockService = distributedLockService;
    private readonly IBlockService _blockService = blockService;
    private readonly IWeb3ProviderService _web3ProviderService = web3ProviderService;

    public async Task<long> Handle(GetNextProcessingBlockQuery request, CancellationToken cancellationToken)
    {
        return await _distributedLockService.RunWithLockAsync(GetNextProcessingBlock(request.Chain, cancellationToken), $"BlockProcessor_NextProcessingBlock_{request.Chain}", cancellationToken: cancellationToken);
    }

    private async Task<long> GetNextProcessingBlock(Nethereum.Signer.Chain chain, CancellationToken cancellationToken)
    {
        var processedBlock = await _context.BlockProgresses.SingleOrDefaultAsync(exp => exp.Chain == chain && exp.Status == Domain.Entities.BlockProgress.BlockProgressStatus.Processed, cancellationToken);
        if(processedBlock is null) 
        {
            var rpcUrls = await _context.RpcUrls.Where(exp => exp.Chain == chain).Select(s => s.Uri).ToListAsync(cancellationToken);
            if(rpcUrls.Count == 0) throw new NotFoundException("No RPC Urls found for the chain");
            var web3 = _web3ProviderService.CreateWeb3(chain, rpcUrls[0].ToString(), cancellationToken);
            var lastBlock = await _blockService.GetLastBlockAsync(web3, cancellationToken);
            processedBlock = new Domain.Entities.BlockProgress
            {
                BlockNumber = (long)lastBlock,
                Chain = chain,
                Status = Domain.Entities.BlockProgress.BlockProgressStatus.Processed
            };
            await _context.BlockProgresses.AddAsync(processedBlock, cancellationToken);
        }
        
        processedBlock.BlockNumber++;
        var processingBlock = new Domain.Entities.BlockProgress
        {
            BlockNumber = processedBlock.BlockNumber,
            Chain = chain,
            Status = Domain.Entities.BlockProgress.BlockProgressStatus.Processing
        };
        await _context.BlockProgresses.AddAsync(processingBlock, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return processedBlock.BlockNumber;
    }
}
