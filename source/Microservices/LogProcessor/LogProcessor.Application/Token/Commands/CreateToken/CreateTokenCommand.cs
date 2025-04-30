using System.ComponentModel.DataAnnotations;
using LogProcessor.Application.Interfaces;
using CacheManager.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogProcessor.Application.WalletAddress.Commands.CreateWalletAddress;


public record CreateTokenCommand([property: Required] Nethereum.Signer.Chain Chain, [property: Required] string ContractAddress,
    string? StakeContractAddress, string? OwnerWalletAddress, string? RoyaltyWalletAddress, int Decimals) : IRequest<Unit>;

public class Handler(IApplicationDbContext context, IDistributedLockService distributedLockService) : IRequestHandler<CreateTokenCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IDistributedLockService _distributedLockService = distributedLockService;

    public async Task<Unit> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
    {
        await _distributedLockService.RunWithLockAsync(AddTokenToDatabaseAsync(request.ContractAddress, request.Chain,
            request.StakeContractAddress, request.OwnerWalletAddress, request.RoyaltyWalletAddress, request.Decimals,
            cancellationToken), "BlockProcessor_CreateWalletAddress", cancellationToken: cancellationToken);
        
        return Unit.Value;
    }

    private async Task AddTokenToDatabaseAsync(string contractAddress, Nethereum.Signer.Chain chain, 
        string? stakeContractAddress, string? ownerWalletAddress, string? royaltyWalletAddress, int decimals,
        CancellationToken cancellationToken)
    {
        if (await _context.Tokens.AnyAsync(x => x.ContractAddress.Equals(contractAddress) && x.Chain == chain, cancellationToken))
            return;

        var entity = new Domain.Entities.Token
        {
            ContractAddress = contractAddress,
            Chain = chain,
            StakeContractAddress = stakeContractAddress,
            OwnerWalletAddress = ownerWalletAddress,
            RoyaltyWalletAddress = royaltyWalletAddress,
            Decimals = decimals
        };
        await _context.Tokens.AddAsync(entity, cancellationToken);

        entity.AddDomainEvent(new Domain.Events.Token.TokenCreatedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);
    }
}
