using BlockProcessor.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlockProcessor.Application.WalletAddress.Commands.CreateWalletAddress;

public class CreateWalletAddressCommandValidator : AbstractValidator<CreateWalletAddressCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateWalletAddressCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(request => request.Address)
            .MustAsync(BeUniqeWalletAddressAsync).WithMessage("Address already exists.");
    }

    private async Task<bool> BeUniqeWalletAddressAsync(string address, CancellationToken cancellationToken)
    {
        return !await _context.WalletAddresses.AnyAsync(exp => exp.Address.Equals(address), cancellationToken);
    }
}
