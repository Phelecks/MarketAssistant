using BaseApplication.Exceptions;
using BaseApplication.Security;
using Informing.Application.Interfaces;
using Informing.Domain.Events.Group;
using MediatR.Interfaces;
using MediatR.Helpers;

namespace Informing.Application.Group.Commands.UpdateGroup;

[Authorize(roles = "Administrators")]
public record UpdateGroupCommand(long id, string? description) : IRequest<Unit>;

public class UpdateGroupCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateGroupCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Unit> HandleAsync(UpdateGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Groups
            .FindAsync([request.id], cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Domain.Entities.Group), request.id);

        entity.Description = request.description;

        entity.AddDomainNotification(new GroupUpdatedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
