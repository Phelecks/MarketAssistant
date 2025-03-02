using BaseApplication.Exceptions;
using BlockProcessor.Application.Interfaces;
using BlockProcessor.Domain.Events.Transfer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlockProcessor.Application.Transfer.Commands.MarkTransferAsConfirmed;

public record MarkTransferAsConfirmedCommand([property: Required] string Hash, [property: Required] Nethereum.Signer.Chain Chain) : IRequest<Unit>;

public class Handler(IApplicationDbContext context) : IRequestHandler<MarkTransferAsConfirmedCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Unit> Handle(MarkTransferAsConfirmedCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Transfers.SingleOrDefaultAsync(exp => exp.Hash.Equals(request.Hash) && exp.Chain == request.Chain, cancellationToken) 
            ?? throw new NotFoundException(nameof(Transfer), request.Hash);
        entity.State = Domain.Entities.Transfer.TransferState.Confirmed;
        entity.AddDomainEvent(new TransferMarkedAsConfirmedEvent(entity));
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
