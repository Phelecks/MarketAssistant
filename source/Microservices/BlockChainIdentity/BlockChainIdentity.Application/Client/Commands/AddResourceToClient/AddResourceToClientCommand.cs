using System.ComponentModel.DataAnnotations;
using BaseApplication.Exceptions;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.Client.Commands.AddResourceToClient;

[Authorize(roles = "Administrators")]
public record AddResourceToClientCommand([property: Required] long Id, [property: Required] List<long> Resources) : IRequest<Unit>;

public class Handler : IRequestHandler<AddResourceToClientCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public Handler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddResourceToClientCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Clients.Include(inc => inc.ClientResources).SingleOrDefaultAsync(exp => exp.Id == request.Id, cancellationToken);
        if (entity == null) throw new NotFoundException(nameof(Domain.Entities.Client), request.Id);

        var resources = await _context.Resources.Where(exp => request.Resources.Any(resourceId => resourceId == exp.Id)).ToListAsync(cancellationToken);

        foreach ( var resource in resources )
        {
            entity.ClientResources.Add(new Domain.Entities.ClientResource
            {
                clientId = entity.Id,
                resourceId = resource.Id
            });
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
