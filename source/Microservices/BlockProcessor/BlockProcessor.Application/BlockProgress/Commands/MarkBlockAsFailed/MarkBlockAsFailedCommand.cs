using System.ComponentModel.DataAnnotations;
using BlockProcessor.Application.Interfaces;
using BlockProcessor.Domain.Events.BlockProgress;
using MediatR.Helpers;
using MediatR.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlockProcessor.Application.BlockProgress.Commands.MarkBlockAsFailed;

public record MarkBlockAsFailedCommand([property: Required] Nethereum.Signer.Chain Chain, [property: Required] long BlockNumber) : IRequest<Unit>;

public class Handler(IApplicationDbContext context) : IRequestHandler<MarkBlockAsFailedCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Unit> HandleAsync(MarkBlockAsFailedCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.BlockProgresses.SingleOrDefaultAsync(exp => exp.Chain == request.Chain && 
            exp.BlockNumber == request.BlockNumber && 
            exp.Status == Domain.Entities.BlockProgress.BlockProgressStatus.Processing, cancellationToken);
            
        if (entity is not null)
        {
            entity.Status = Domain.Entities.BlockProgress.BlockProgressStatus.Failed;
            entity.AddDomainNotification(new BlockFailedEvent(entity));
            await _context.SaveChangesAsync(cancellationToken);
        }
        
        return Unit.Value;
    }
}