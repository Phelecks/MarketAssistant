using System.ComponentModel.DataAnnotations;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using MediatR.Interfaces;
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

    public async Task<long> HandleAsync(CreateTokenCommand request, CancellationToken cancellationToken)
    {
        var client = await _context.Clients.Include(inc => inc.ClientResources).ThenInclude(inc => inc.Resource).SingleAsync(exp => exp.Id == request.ClientId, cancellationToken);

        var entity = new Domain.Entities.Token
        {
            IssuedAt = request.IssuedAt,
            ExpireAt = request.ExpireAt,
            Enabled = request.Enabled,
            Nonce = request.Nonce,
            NotBefore = request.NotBefore,
            RequestId = request.RequestId,
            statement = request.Statement,
            version = request.Version,
            WalletAddress = request.WalletAddress,
            Resources = string.Join(',', client.ClientResources.Select(s => s.Resource.Title).ToList()),
            Uri = request.Uri
        };

        await _context.Tokens.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
