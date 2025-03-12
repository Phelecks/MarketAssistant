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

        var client = await _context.clients.SingleAsync(exp => exp.ClientId.Equals(clientResult.data.ClientId) &&
            exp.ClientSecret.Equals(clientResult.data.ClientSecret) && exp.Enabled, cancellationToken);

        var siweMessage = _identityService.GenerateSiweMessage(request.Address, client.Uri, client.Statement, client.Version, request.chainId, Guid.NewGuid().ToString(), _dateTimeService.UtcNow.AddSeconds(client.TokenLifeTimeInSeconds));

        return new SiweMessageDto(siweMessage);
    }
}
