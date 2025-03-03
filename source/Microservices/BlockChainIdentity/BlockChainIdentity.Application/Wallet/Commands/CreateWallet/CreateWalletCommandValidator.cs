using BlockChainIdentity.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.Wallet.Commands.CreateWallet;

public class CreateWalletCommandValidator : AbstractValidator<CreateWalletCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateWalletCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(request => request.Address)
            .NotEmpty().WithMessage("Address cannot be empty.")
            .NotNull().WithMessage("Address cannot be null.");

        RuleFor(request => request.ClientId)
            .NotEmpty().WithMessage("ClientId cannot be empty.")
            .NotNull().WithMessage("ClientId cannot be null.")
            .MustAsync(BeClientExistsAsync).WithMessage("Client does not exist.");
    }

    async Task<bool> BeClientExistsAsync(long clientId, CancellationToken cancellationToken)
    {
        return await _context.clients.AnyAsync(exp => exp.Id == clientId, cancellationToken);
    }
}
