using BaseApplication.Exceptions;
using Informing.Application.Interfaces;
using MediatR;

namespace Informing.Application.Group.Commands.DeleteGroup;


public record DeleteGroupCommand(long id) : IRequest<Unit>;

public class DeleteGroupCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteGroupCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Unit> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Groups
            .FindAsync([request.id], cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Group), request.id);
        }

        _context.Groups.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
