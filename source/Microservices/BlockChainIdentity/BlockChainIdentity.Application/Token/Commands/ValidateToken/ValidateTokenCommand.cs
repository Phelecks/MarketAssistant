using System.ComponentModel.DataAnnotations;
using BlockChainIdentity.Application.Interfaces;
using MediatR;

namespace BlockChainIdentity.Application.Token.Commands.ValidateToken;

//[Authorize(roles = "Administrators")]
public record ValidateTokenCommand([property: Required] string token) : IRequest<Unit>;

public class Handler(IIdentityService identityService) : IRequestHandler<ValidateTokenCommand, Unit>
{
    private readonly IIdentityService _identityService = identityService;

    public async Task<Unit> Handle(ValidateTokenCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.ValidateTokenAsync(request.token, cancellationToken);
        if(!result.IsSuccess())
            throw new UnauthorizedAccessException(result.ResponseInformation);

        return Unit.Value;
    }
}
