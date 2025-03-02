using BaseApplication.Exceptions;
using BlockProcessor.Application.Interfaces;
using BlockProcessor.Domain.Events.Transfer;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BlockProcessor.Application.Transfer.Commands.MarkTransferAsFailed;

public record MarkTransferAsFailed([property: Required] string Hash, [property: Required] Nethereum.Signer.Chain Chain) : IRequest<Unit>;

public class Handler(IApplicationDbContext context) : IRequestHandler<MarkTransferAsFailed, Unit>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Unit> Handle(MarkTransferAsFailed request, CancellationToken cancellationToken)
    {
        var entity = await _context.Transfers.FindAsync(request.Hash, request.Chain, cancellationToken) ?? throw new NotFoundException(nameof(Transfer), request.Hash);
        entity.State = Domain.Entities.Transfer.TransferState.Failed;
        entity.AddDomainEvent(new TransferMarkedAsFailedEvent(entity));
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
