using System.ComponentModel.DataAnnotations;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using MediatR;

namespace BlockChainIdentity.Application.Role.Commands.CreateRole;

[Authorize(roles = "Administrators")]
public record CreateRoleCommand([property: Required] string Title) : IRequest<long>;

public class Handler : IRequestHandler<CreateRoleCommand, long>
{
    private readonly IApplicationDbContext _context;

    public Handler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Role
        {
            title = request.Title,
        };

        await _context.Roles.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
