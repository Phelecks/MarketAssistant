﻿using BlockChainIdentity.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.Token.Commands.CreateToken;

public class CreateTokenCommandValidator : AbstractValidator<CreateTokenCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateTokenCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(request => request.WalletAddress)
            .MustAsync(BeWalletExistsAsync).WithMessage("Wallet address does not exist.");

        RuleFor(request => request.ClientId)
            .MustAsync(BeValidClientAsync).WithMessage("Client does not exist or disabled.");
    }

    async Task<bool> BeWalletExistsAsync(string WalletAddress, CancellationToken cancellationToken)
    {
        return await _context.Wallets.AnyAsync(exp => exp.Address.Equals(WalletAddress), cancellationToken);
    }

    async Task<bool> BeValidClientAsync(long ClientId, CancellationToken cancellationToken)
    {
        var result = await _context.Clients.SingleOrDefaultAsync(exp => exp.Id == ClientId, cancellationToken);
        if (result == null) return false;
        return result.Enabled;
    }
}
