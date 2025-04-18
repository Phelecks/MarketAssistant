﻿using BaseApplication.Exceptions;
using Informing.Application.Interfaces;
using MediatR;

namespace Informing.Application.Group.Commands.DeleteGroup;


public record DeleteGroupCommand(long id) : IRequest<Unit>;

public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    //public async Task<Unit> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
    //{
    //    var entity = await _context.groups
    //        .FindAsync(new object[] { request.id }, cancellationToken);

    //    if (entity == null)
    //    {
    //        throw new NotFoundException(nameof(Domain.Entities.Group), request.id);
    //    }

    //    _context.groups.Remove(entity);

    //    await _context.SaveChangesAsync(cancellationToken);

    //    return Unit.Value;
    //}
    public async Task<Unit> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Groups
            .FindAsync(new object[] { request.id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Group), request.id);
        }

        _context.Groups.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
