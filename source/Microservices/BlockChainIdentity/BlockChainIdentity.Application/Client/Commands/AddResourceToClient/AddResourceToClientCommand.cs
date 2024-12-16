﻿using System.ComponentModel.DataAnnotations;
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
        var entity = await _context.clients.Include(inc => inc.clientResources).SingleOrDefaultAsync(exp => exp.id == request.Id, cancellationToken);
        if (entity == null) throw new NotFoundException(nameof(Domain.Entities.Client), request.Id);

        var resources = await _context.resources.Where(exp => request.Resources.Any(resourceId => resourceId == exp.id)).ToListAsync(cancellationToken);

        foreach ( var resource in resources )
        {
            entity.clientResources.Add(new Domain.Entities.ClientResource
            {
                clientId = entity.id,
                resourceId = resource.id
            });
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}