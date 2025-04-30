using FluentValidation;
using LogProcessor.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LogProcessor.Application.Transfer.Commands.InitiateTransfer;

public class InitiateTransactionCommandValidator : AbstractValidator<InitiateTransferCommand>
{
    private readonly IApplicationDbContext _context;

    public InitiateTransactionCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(request => request)
            .MustAsync(BeUniqueHashInChainAsync).WithMessage("The specified hash already exists.");
    }

    private async Task<bool> BeUniqueHashInChainAsync(InitiateTransferCommand request, CancellationToken cancellationToken)
    {
        return !await _context.Transfers.AnyAsync(exp => exp.Hash.Equals(request.Hash) && exp.Chain == request.Chain, cancellationToken);
    }
}
