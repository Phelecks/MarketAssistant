using BlockChainIdentity.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.WalletRole.Commands.RemoveWalletFromRole;

public class RemoveWalletFromRoleCommandValidator : AbstractValidator<RemoveWalletFromRoleCommand>
{
    private readonly IApplicationDbContext _context;

    public RemoveWalletFromRoleCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(request => request.WalletAddress)
            .NotEmpty().WithMessage("Address cannot be empty.")
            .NotNull().WithMessage("Address cannot be null.")
            .MustAsync(BeWalletExistsAsync).WithMessage("Wallet does not exist.");

        RuleFor(request => request.RoleId)
            .MustAsync(BeRoleExistsAsync).WithMessage("Role does not exist.");

        RuleFor(request => request)
            .MustAsync(BeWaletRoleExistsAsync).WithMessage("Wallet does not belong to selected role.");
    }

    async Task<bool> BeWalletExistsAsync(string walletAddress, CancellationToken cancellationToken)
    {
        return await _context.wallets.AnyAsync(exp => exp.address.Equals(walletAddress), cancellationToken);
    }

    async Task<bool> BeRoleExistsAsync(long roleId, CancellationToken cancellationToken)
    {
        return await _context.roles.AnyAsync(exp => exp.Id == roleId, cancellationToken);
    }

    async Task<bool> BeWaletRoleExistsAsync(RemoveWalletFromRoleCommand request, CancellationToken cancellationToken)
    {
        return await _context.walletRoles.AnyAsync(exp => exp.Id == request.RoleId && exp.walletAddress.Equals(request.WalletAddress), cancellationToken);
    }
}
