using System.ComponentModel.DataAnnotations;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using MediatR.Helpers;
using MediatR.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.Token.Commands.RevokeTokens;

[Authorize(roles = "Administrators")]
public record RevokeTokensCommand([property: Required] string WalletAddress) : IRequest<Unit>;

public class Handler : IRequestHandler<RevokeTokensCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public Handler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> HandleAsync(RevokeTokensCommand request, CancellationToken cancellationToken)
    {
        var tokens = await _context.Tokens.Where(exp => exp.WalletAddress.Equals(request.WalletAddress)).ToListAsync(cancellationToken);
        tokens.ForEach(item => item.Enabled = false);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
