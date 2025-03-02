using BlockProcessor.Application.Interfaces;
using CacheManager.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlockProcessor.Application.BlockProgress.Queries.GetNextProcessingBlock;

public record GetNextProcessingBlockQuery([property: Required] Nethereum.Signer.Chain Chain) : IRequest<long>;

public class Handler(IApplicationDbContext context, IDistributedLockService<long> distributedLockService) : IRequestHandler<GetNextProcessingBlockQuery, long>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IDistributedLockService<long> _distributedLockService = distributedLockService;

    public async Task<long> Handle(GetNextProcessingBlockQuery request, CancellationToken cancellationToken)
    {
        return await _distributedLockService.RunWithLockAsync(GetNextProcessingBlock(request.Chain, cancellationToken), $"BlockProcessor_NextProcessingBlock_{request.Chain}", cancellationToken: cancellationToken);
    }

    private async Task<long> GetNextProcessingBlock(Nethereum.Signer.Chain chain, CancellationToken cancellationToken)
    {
        var processedBlock = await _context.BlockProgresses.SingleOrDefaultAsync(exp => exp.Chain == chain && exp.Status == Domain.Entities.BlockProgress.BlockProgressStatus.Processed, cancellationToken);
        if(processedBlock is null) 
        {
            processedBlock = new Domain.Entities.BlockProgress
            {
                BlockNumber = 0,
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
