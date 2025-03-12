using BaseApplication.Exceptions;
using BlockChainIdentity.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.Wallet.Queries.GetSiweMessage;

public class GetSiweMessageQueryValidator : AbstractValidator<GetSiweMessageQuery>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public GetSiweMessageQueryValidator(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;

        RuleFor(request => request.Address)
            .NotEmpty().WithMessage("Address cannot be empty.")
            .NotNull().WithMessage("Address cannot be null.");

        RuleFor(request => request.ClientKey)
            .NotEmpty().WithMessage("ClientKey cannot be empty.")
            .NotNull().WithMessage("ClientKey cannot be null.")
            .MustAsync(BeClientValidAsync).WithMessage("Client key is not valid or does not exist.");
    }

    async Task<bool> BeClientValidAsync(string clientKey, CancellationToken cancellationToken)
    {
        var clientResult = _identityService.GetClient(clientKey);
        if (!clientResult.IsSuccess()) return false;

        var client = await _context.clients.SingleOrDefaultAsync(exp => exp.ClientId.Equals(clientResult.data.ClientId) && 
            exp.ClientSecret.Equals(clientResult.data.ClientSecret) && exp.Enabled, cancellationToken);
        if(client == null) return false;
        return true;
    }
}
