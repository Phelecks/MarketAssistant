using System.ComponentModel.DataAnnotations;
using BlockChainIdentity.Application.Interfaces;
using MediatR;

namespace BlockChainIdentity.Application.Token.Commands.ValidateToken;

//[Authorize(roles = "Administrators")]
public record ValidateTokenCommand([property: Required] string token) : IRequest<Unit>;

public class Handler : IRequestHandler<ValidateTokenCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public Handler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<Unit> Handle(ValidateTokenCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.ValidateTokenAsync(request.token, cancellationToken);
        if(!result.IsSuccess())
            throw new UnauthorizedAccessException(result.ResponseInformation);

        return Unit.Value;
    }
}
