using System.ComponentModel.DataAnnotations;
using LogProcessor.Domain.Events.BlockProgress;
using LogProcessor.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogProcessor.Application.BlockProgress.Commands.MarkBlockAsProcessed;

public record MarkBlockAsProcessedCommand([property: Required] Nethereum.Signer.Chain Chain, [property: Required] long BlockNumber) : IRequest<Unit>;

public class Handler(IApplicationDbContext context) : IRequestHandler<MarkBlockAsProcessedCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Unit> Handle(MarkBlockAsProcessedCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.BlockProgresses.SingleOrDefaultAsync(exp => exp.Chain == request.Chain && 
            exp.BlockNumber == request.BlockNumber && 
            exp.Status == Domain.Entities.BlockProgress.BlockProgressStatus.Processing, cancellationToken);
            
        if (entity is not null)
        {
            entity.Status = Domain.Entities.BlockProgress.BlockProgressStatus.Processed;
            entity.AddDomainEvent(new BlockProcessedEvent(entity));
            await _context.SaveChangesAsync(cancellationToken);
        }
        
        return Unit.Value;
    }
}