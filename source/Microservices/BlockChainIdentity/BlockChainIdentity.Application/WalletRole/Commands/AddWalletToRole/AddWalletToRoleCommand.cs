using System.ComponentModel.DataAnnotations;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using MediatR;

namespace BlockChainIdentity.Application.WalletRole.Commands.AddWalletToRole;

[Authorize(roles = "Administrators")]
public record AddWalletToRoleCommand([property: Required] string WalletAddress, [property: Required] long RoleId) : IRequest<long>;

public class Handler : IRequestHandler<AddWalletToRoleCommand, long>
{
    private readonly IApplicationDbContext _context;

    public Handler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(AddWalletToRoleCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.WalletRole
        {
            walletAddress = request.WalletAddress,
            roleId = request.RoleId
        };

        await _context.walletRoles.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
