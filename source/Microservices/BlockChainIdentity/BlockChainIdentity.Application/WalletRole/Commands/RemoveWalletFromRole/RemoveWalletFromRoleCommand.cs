using System.ComponentModel.DataAnnotations;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using MediatR;

namespace BlockChainIdentity.Application.WalletRole.Commands.RemoveWalletFromRole;

[Authorize(roles = "Administrators")]
public record RemoveWalletFromRoleCommand([property: Required] string WalletAddress, [property: Required] long RoleId) : IRequest<Unit>;

public class Handler : IRequestHandler<RemoveWalletFromRoleCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public Handler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RemoveWalletFromRoleCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.WalletRole
        {
            WalletAddress = request.WalletAddress,
            RoleId = request.RoleId
        };

        _context.WalletRoles.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
