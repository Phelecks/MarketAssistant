using System.ComponentModel.DataAnnotations;
using BlockProcessor.Application.Interfaces;
using CacheManager.Interfaces;
using MediatR.Helpers;
using MediatR.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlockProcessor.Application.WalletAddress.Commands.DeleteWalletAddress;


public record DeleteWalletAddressCommand([property: Required] string Address) : IRequest<Unit>;

public class Handler(IApplicationDbContext context, IDistributedLockService distributedLockService) : IRequestHandler<DeleteWalletAddressCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IDistributedLockService _distributedLockService = distributedLockService;

    public async Task<Unit> HandleAsync(DeleteWalletAddressCommand request, CancellationToken cancellationToken)
    {
        await _distributedLockService.RunWithLockAsync(RemoveWalletAddressFromDatabaseAsync(request.Address, cancellationToken), "BlockProcessor_DeleteWalletAddress", cancellationToken: cancellationToken);
        
        return Unit.Value;
    }

    private async Task RemoveWalletAddressFromDatabaseAsync(string address, CancellationToken cancellationToken)
    {
        var entity = await _context.WalletAddresses.SingleOrDefaultAsync(exp => exp.Address.Equals(address), cancellationToken);
        if(entity is null) return;

        _context.WalletAddresses.Remove(entity);

        entity.AddDomainNotification(new Domain.Events.WalletAddress.WalletAddressDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);
    }
}
