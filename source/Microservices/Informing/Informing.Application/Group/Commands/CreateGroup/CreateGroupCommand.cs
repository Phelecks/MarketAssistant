﻿using System.ComponentModel.DataAnnotations;
using MediatR;
using IApplicationDbContext = Informing.Application.Interfaces.IApplicationDbContext;

namespace Informing.Application.Group.Commands.CreateGroup;


public record CreateGroupCommand([property: Required] string title, string description) : IRequest<long>;

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, long>
{
    private readonly IApplicationDbContext _context;

    public CreateGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Group
        {
            title = request.title,
            description = request.description
        };

        _context.groups.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.id;
    }
}
