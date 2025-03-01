using BlockProcessor.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlockProcessor.Application.BlockProgress.Queries.GetNextProcessingBlock;

public record GetNextProcessingBlockQuery([property: Required] Nethereum.Signer.Chain Chain) : IRequest<long>;

public class Handler(IApplicationDbContext context) : IRequestHandler<GetNextProcessingBlockQuery, long>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<long> Handle(GetNextProcessingBlockQuery request, CancellationToken cancellationToken)
    {
        var lastProcessedBlock = await _context.BlockProgresses.SingleOrDefaultAsync(exp => exp.Chain == request.Chain, cancellationToken);

        return lastProcessedBlock == null ? 1 : lastProcessedBlock.BlockNumber + 1;
    }
}
