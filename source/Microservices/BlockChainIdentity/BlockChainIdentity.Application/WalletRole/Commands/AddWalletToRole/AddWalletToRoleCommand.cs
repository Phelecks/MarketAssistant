using System.ComponentModel.DataAnnotations;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using MediatR.Interfaces;

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

    public async Task<long> HandleAsync(AddWalletToRoleCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.WalletRole
        {
            WalletAddress = request.WalletAddress,
            RoleId = request.RoleId
        };

        await _context.WalletRoles.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
