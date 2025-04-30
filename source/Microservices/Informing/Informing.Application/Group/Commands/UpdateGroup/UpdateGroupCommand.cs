using BaseApplication.Exceptions;
using BaseApplication.Security;
using Informing.Application.Interfaces;
using Informing.Domain.Events.Group;
using MediatR;

namespace Informing.Application.Group.Commands.UpdateGroup;

[Authorize(roles = "Administrators")]
public record UpdateGroupCommand(long id, string? description) : IRequest<Unit>;

public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    //public async Task<Unit> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
    //{
    //    var entity = await _context.groups
    //        .FindAsync(new object[] { request.id }, cancellationToken);

    //    if (entity == null)
    //        throw new NotFoundException(nameof(Domain.Entities.Group), request.id);

    //    entity.description = request.description;

    //    entity.AddDomainEvent(new GroupUpdatedEvent(entity));

    //    await _context.SaveChangesAsync(cancellationToken);

    //    return Unit.Value;
    //}
    public async Task<Unit> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Groups
            .FindAsync([request.id], cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Domain.Entities.Group), request.id);

        entity.Description = request.description;

        entity.AddDomainEvent(new GroupUpdatedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
