using System.ComponentModel.DataAnnotations;
using BlockProcessor.Application.Interfaces;
using MediatR;

namespace BlockProcessor.Application.WalletAddress.Commands.CreateWalletAddress;


public record CreateWalletAddressCommand([property: Required] string Address) : IRequest<Unit>;

public class Handler(IApplicationDbContext context) : IRequestHandler<CreateWalletAddressCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Unit> Handle(CreateWalletAddressCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.WalletAddress
        {
            Address = request.Address
        };
        await _context.WalletAddresses.AddAsync(entity, cancellationToken);

        entity.AddDomainEvent(new Domain.Events.WalletAddress.WalletAddressCreatedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
