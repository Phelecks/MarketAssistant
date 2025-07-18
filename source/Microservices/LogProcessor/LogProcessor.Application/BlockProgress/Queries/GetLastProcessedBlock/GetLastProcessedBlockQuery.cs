using BaseApplication.Exceptions;
using BlockChainQueryHelper.Interfaces;
using BlockChainWeb3ProviderHelper.Interfaces;
using LogProcessor.Application.Interfaces;
using MediatR.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LogProcessor.Application.BlockProgress.Queries.GetLastProcessedBlock;

public record GetLastProcessedBlockQuery([property: Required] Nethereum.Signer.Chain Chain) : IRequest<long>;

public class Handler(IApplicationDbContext context, IBlockService blockService, IWeb3ProviderService web3ProviderService) : IRequestHandler<GetLastProcessedBlockQuery, long>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IBlockService _blockService = blockService;
    private readonly IWeb3ProviderService _web3ProviderService = web3ProviderService;

    public async Task<long> HandleAsync(GetLastProcessedBlockQuery request, CancellationToken cancellationToken)
    {
        var processedBlock = await _context.BlockProgresses.SingleOrDefaultAsync(exp => exp.Chain == request.Chain && exp.Status == Domain.Entities.BlockProgress.BlockProgressStatus.Processed, cancellationToken);
        if(processedBlock is null) 
        {
            var rpcUrls = await _context.RpcUrls.Where(exp => exp.Chain == request.Chain).Select(s => new { s.Uri, s.BlockOfConfirmation }).ToListAsync(cancellationToken);
            if(rpcUrls.Count == 0) throw new NotFoundException("No RPC Urls found for the chain");
            var web3 = _web3ProviderService.CreateWeb3(rpcUrls[0].Uri.ToString());
            var lastBlock = await _blockService.GetLastBlockAsync(web3, cancellationToken);
            processedBlock = new Domain.Entities.BlockProgress
            {
                BlockNumber = (long)lastBlock - rpcUrls[0].BlockOfConfirmation,
                Chain = request.Chain,
                Status = Domain.Entities.BlockProgress.BlockProgressStatus.Processed
            };
            await _context.BlockProgresses.AddAsync(processedBlock, cancellationToken);
        }

        return processedBlock.BlockNumber;
    }
}
