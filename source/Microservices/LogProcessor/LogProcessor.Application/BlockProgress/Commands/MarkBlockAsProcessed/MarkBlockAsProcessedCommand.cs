using System.ComponentModel.DataAnnotations;
using LogProcessor.Domain.Events.BlockProgress;
using LogProcessor.Application.Interfaces;
using MediatR.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR.Helpers;

namespace LogProcessor.Application.BlockProgress.Commands.MarkBlockAsProcessed;

public record MarkBlockAsProcessedCommand([property: Required] Nethereum.Signer.Chain Chain, [property: Required] long BlockNumber) : IRequest<Unit>;

public class Handler(IApplicationDbContext context) : IRequestHandler<MarkBlockAsProcessedCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Unit> HandleAsync(MarkBlockAsProcessedCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.BlockProgresses.SingleOrDefaultAsync(exp => exp.Chain == request.Chain && 
            exp.BlockNumber == request.BlockNumber && 
            exp.Status == Domain.Entities.BlockProgress.BlockProgressStatus.Processing, cancellationToken);
            
        if (entity is not null)
        {
            entity.Status = Domain.Entities.BlockProgress.BlockProgressStatus.Processed;
            entity.AddDomainNotification(new BlockProcessedEvent(entity));
            await _context.SaveChangesAsync(cancellationToken);
        }
        
        return Unit.Value;
    }
}