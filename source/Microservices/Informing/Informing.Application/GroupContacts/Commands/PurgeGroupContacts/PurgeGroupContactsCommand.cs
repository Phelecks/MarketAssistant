using BaseApplication.Security;
using Informing.Application.Interfaces;
using MediatR;

namespace Informing.Application.GroupContacts.Commands.PurgeGroupContacts;

[Authorize(roles = "Administrators")]
public record PurgeGroupContactsCommand : IRequest<Unit>;

public class PurgeGroupContactsCommandHandler : IRequestHandler<PurgeGroupContactsCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public PurgeGroupContactsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    //public async Task<Unit> Handle(PurgeGroupContactsCommand request, CancellationToken cancellationToken)
    //{
    //    _context.groupContacts.RemoveRange(_context.groupContacts);

    //    await _context.SaveChangesAsync(cancellationToken);

    //    return Unit.Value;
    //}
    public async Task<Unit> Handle(PurgeGroupContactsCommand request, CancellationToken cancellationToken)
    {
        _context.GroupContacts.RemoveRange(_context.GroupContacts);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
