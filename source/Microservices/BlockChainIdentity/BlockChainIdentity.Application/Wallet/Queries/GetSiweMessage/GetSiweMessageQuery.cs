using BaseApplication.Exceptions;
using BaseApplication.Interfaces;
using BlockChainIdentity.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using IIdentityService = BlockChainIdentity.Application.Interfaces.IIdentityService;

namespace BlockChainIdentity.Application.Wallet.Queries.GetSiweMessage;

//[Authorize(roles = "Administrators")]
public record GetSiweMessageQuery([property: Required] string Address, [property: Required] string ClientKey, [property: Required] int chainId) : IRequest<SiweMessageDto>;

public class Handler : IRequestHandler<GetSiweMessageQuery, SiweMessageDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IDateTimeService _dateTimeService;

    public Handler(IApplicationDbContext context, IIdentityService identityService, IDateTimeService dateTimeService)
    {
        _context = context;
        _identityService = identityService;
        _dateTimeService = dateTimeService;
    }

    public async Task<SiweMessageDto> Handle(GetSiweMessageQuery request, CancellationToken cancellationToken)
    {
        var clientResult = _identityService.GetClient(request.ClientKey);

        var client = await _context.clients.SingleAsync(exp => exp.clientId.Equals(clientResult.data.ClientId) &&
            exp.clientSecret.Equals(clientResult.data.ClientSecret) && exp.enabled, cancellationToken);

        var siweMessage = _identityService.GenerateSiweMessage(request.Address, client.uri, client.statement, client.version, request.chainId, Guid.NewGuid().ToString(), _dateTimeService.UtcNow.AddSeconds(client.tokenLifeTimeInSeconds));

        return new SiweMessageDto(siweMessage);
    }
}
