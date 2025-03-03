using System.ComponentModel.DataAnnotations;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.Token.Commands.CreateToken;

[Authorize(roles = "Administrators")]
public record CreateTokenCommand([property: Required] DateTime IssuedAt, [property: Required] DateTime ExpireAt, 
    [property: Required] DateTime NotBefore, [property: Required] string Statement, [property: Required] Uri Uri,
    [property: Required] string Version, [property: Required] string Nonce, [property: Required] string RequestId,
    [property: Required] bool Enabled, [property: Required] string WalletAddress, [property: Required] long ClientId) : IRequest<long>;

public class Handler : IRequestHandler<CreateTokenCommand, long>
{
    private readonly IApplicationDbContext _context;

    public Handler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
    {
        var client = await _context.clients.Include(inc => inc.clientResources).ThenInclude(inc => inc.resource).SingleAsync(exp => exp.Id == request.ClientId, cancellationToken);

        var entity = new Domain.Entities.Token
        {
            issuedAt = request.IssuedAt,
            expireAt = request.ExpireAt,
            enabled = request.Enabled,
            nonce = request.Nonce,
            notBefore = request.NotBefore,
            requestId = request.RequestId,
            statement = request.Statement,
            version = request.Version,
            walletAddress = request.WalletAddress,
            resources = string.Join(',', client.clientResources.Select(s => s.resource.title).ToList()),
            uri = request.Uri
        };

        await _context.tokens.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
