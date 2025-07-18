using System.ComponentModel.DataAnnotations;
using BlockProcessor.Application.Interfaces;
using CacheManager.Interfaces;
using MediatR.Helpers;
using MediatR.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlockProcessor.Application.WalletAddress.Commands.CreateWalletAddress;


public record CreateWalletAddressCommand([property: Required] string Address) : IRequest<Unit>;

public class Handler(IApplicationDbContext context, IDistributedLockService distributedLockService) : IRequestHandler<CreateWalletAddressCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IDistributedLockService _distributedLockService = distributedLockService;

    public async Task<Unit> HandleAsync(CreateWalletAddressCommand request, CancellationToken cancellationToken)
    {
        await _distributedLockService.RunWithLockAsync(AddWalletAddressToDatabaseAsync(request.Address, cancellationToken), "BlockProcessor_CreateWalletAddress", cancellationToken: cancellationToken);
        
        return Unit.Value;
    }

    private async Task AddWalletAddressToDatabaseAsync(string address, CancellationToken cancellationToken)
    {
        if (await _context.WalletAddresses.AnyAsync(x => x.Address.Equals(address), cancellationToken))
            return;

        var entity = new Domain.Entities.WalletAddress
        {
            Address = address
        };
        await _context.WalletAddresses.AddAsync(entity, cancellationToken);

        entity.AddDomainNotification(new Domain.Events.WalletAddress.WalletAddressCreatedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);
    }
}
