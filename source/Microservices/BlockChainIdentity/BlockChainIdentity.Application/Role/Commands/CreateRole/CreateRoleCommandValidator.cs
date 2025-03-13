using BlockChainIdentity.Application.Interfaces;
using BlockChainIdentity.Application.Role.Commands.CreateRole;
using BlockChainIdentity.Application.WalletRole.Commands.AddWalletToRole;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Role.Wallet.CreateRole.CreateWallet;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateRoleCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(request => request.Title)
            .NotEmpty().WithMessage("Address cannot be empty.")
            .NotNull().WithMessage("Address cannot be null.")
            .MustAsync(BeUniqueRoleTitleAsync).WithMessage("The role with this title already exists.");
    }

    async Task<bool> BeUniqueRoleTitleAsync(string title, CancellationToken cancellationToken)
    {
        return await _context.Roles.AllAsync(l => !l.title.Equals(title), cancellationToken);
    }
}
