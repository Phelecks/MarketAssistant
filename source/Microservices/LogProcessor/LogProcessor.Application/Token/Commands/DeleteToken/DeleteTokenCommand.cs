using System.ComponentModel.DataAnnotations;
using LogProcessor.Application.Interfaces;
using CacheManager.Interfaces;
using MediatR.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR.Helpers;

namespace LogProcessor.Application.Token.Commands.DeleteToken;

public record DeleteTokenCommand([property: Required] string ContractAddress, [property: Required] Nethereum.Signer.Chain Chain) : IRequest<Unit>;

public class Handler(IApplicationDbContext context, IDistributedLockService distributedLockService) : IRequestHandler<DeleteTokenCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IDistributedLockService _distributedLockService = distributedLockService;

    public async Task<Unit> HandleAsync(DeleteTokenCommand request, CancellationToken cancellationToken)
    {
        await _distributedLockService.RunWithLockAsync(RemoveTokenFromDatabaseAsync(request.ContractAddress, request.Chain, cancellationToken), "BlockProcessor_DeleteWalletAddress", cancellationToken: cancellationToken);
        
        return Unit.Value;
    }

    private async Task RemoveTokenFromDatabaseAsync(string address, Nethereum.Signer.Chain chain, CancellationToken cancellationToken)
    {
        var entity = await _context.Tokens.SingleOrDefaultAsync(exp => exp.ContractAddress.Equals(address) && exp.Chain == chain, cancellationToken);
        if(entity is null) return;

        _context.Tokens.Remove(entity);

        entity.AddDomainNotification(new Domain.Events.Token.TokenDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);
    }
}
