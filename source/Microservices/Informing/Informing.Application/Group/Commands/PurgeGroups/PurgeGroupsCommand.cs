using BaseApplication.Security;
using Informing.Application.Interfaces;
using MediatR;

namespace Informing.Application.Group.Commands.PurgeGroups;

[Authorize(roles = "Administrators")]
public record PurgeGroupsCommand : IRequest<Unit>;

public class PurgeGroupsCommandHandler : IRequestHandler<PurgeGroupsCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public PurgeGroupsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    //public async Task<Unit> Handle(PurgeGroupsCommand request, CancellationToken cancellationToken)
    //{
    //    _context.groups.RemoveRange(_context.groups);

    //    await _context.SaveChangesAsync(cancellationToken);

    //    return Unit.Value;
    //}
    public async Task<Unit> Handle(PurgeGroupsCommand request, CancellationToken cancellationToken)
    {
        _context.Groups.RemoveRange(_context.Groups);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
