﻿using System.ComponentModel.DataAnnotations;
using BlockChainIdentity.Application.Interfaces;
using BlockChainIdentity.Domain.Events.Wallet;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.Wallet.Commands.CreateWallet;

//[Authorize(roles = "Administrators")]
public record CreateWalletCommand([property: Required] string Address, [property: Required] int ChainId, long ClientId) : IRequest<string>;

public class Handler : IRequestHandler<CreateWalletCommand, string>
{
    private readonly IApplicationDbContext _context;

    public Handler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        var client = await _context.Clients.SingleAsync(exp => exp.Id == request.ClientId, cancellationToken);

        var entity = new Domain.Entities.Wallet
        {
            Address = request.Address,
            ChainId = request.ChainId
        };

        await _context.Wallets.AddAsync(entity, cancellationToken);

        entity.AddDomainEvent(new WalletCreatedEvent(entity, client.ClientId));

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Address;
    }
}
