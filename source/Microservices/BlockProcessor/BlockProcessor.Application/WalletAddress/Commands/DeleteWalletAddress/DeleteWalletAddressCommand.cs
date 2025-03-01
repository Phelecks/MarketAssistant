using System.ComponentModel.DataAnnotations;
using BaseApplication.Exceptions;
using BlockProcessor.Application.Interfaces;
using MediatR;

namespace BlockProcessor.Application.WalletAddress.Commands.DeleteWalletAddress;


public record DeleteWalletAddressCommand([property: Required] long Id) : IRequest<Unit>;

public class Handler(IApplicationDbContext context) : IRequestHandler<DeleteWalletAddressCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Unit> Handle(DeleteWalletAddressCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.WalletAddresses.FindAsync(request.Id) ?? throw new NotFoundException(nameof(WalletAddress), request.Id);
        
        _context.WalletAddresses.Remove(entity);

        entity.AddDomainEvent(new Domain.Events.WalletAddress.WalletAddressDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
