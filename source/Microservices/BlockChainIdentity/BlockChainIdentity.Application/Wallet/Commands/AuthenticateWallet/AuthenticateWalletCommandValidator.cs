using BaseApplication.Exceptions;
using BlockChainIdentity.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.Wallet.Commands.AuthenticateWallet;

public class AuthenticateWalletCommandValidator : AbstractValidator<AuthenticateWalletCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public AuthenticateWalletCommandValidator(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;

        RuleFor(request => request.SiweEncodedMessage)
            .NotEmpty().WithMessage("Address cannot be empty.")
            .NotNull().WithMessage("Address cannot be null.");

        RuleFor(request => request.Signature)
           .NotEmpty().WithMessage("Address cannot be empty.")
           .NotNull().WithMessage("Address cannot be null.");

        RuleFor(request => request.ClientKey)
             .NotEmpty().WithMessage("ClientKey cannot be empty.")
             .NotNull().WithMessage("ClientKey cannot be null.")
             .MustAsync(BeClientValidAsync).WithMessage("The specified field already exists.");
        
    }

    async Task<bool> BeClientValidAsync(string clientKey, CancellationToken cancellationToken)
    {
        var clientResult = _identityService.GetClient(clientKey);
        if (!clientResult.IsSuccess()) throw new ForbiddenAccessException();

        var client = await _context.Clients.SingleOrDefaultAsync(exp => exp.ClientId.Equals(clientResult.data.ClientId) &&
            exp.ClientSecret.Equals(clientResult.data.ClientSecret) && exp.Enabled, cancellationToken);
        if (client == null) throw new ForbiddenAccessException();
        return true;
    }
}
