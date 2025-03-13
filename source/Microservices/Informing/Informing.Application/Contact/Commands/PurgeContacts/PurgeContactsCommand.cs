using BaseApplication.Security;
using Informing.Application.Interfaces;
using MediatR;

namespace Informing.Application.Contact.Commands.PurgeContacts;

[Authorize(roles = "Administrators")]
public record PurgeContactsCommand : IRequest<Unit>;

public class PurgeContactsCommandHandler : IRequestHandler<PurgeContactsCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public PurgeContactsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    //public async Task<Unit> Handle(PurgeContactsCommand request, CancellationToken cancellationToken)
    //{
    //    _context.contacts.RemoveRange(_context.contacts);

    //    await _context.SaveChangesAsync(cancellationToken);

    //    return Unit.Value;
    //}
    public async Task<Unit> Handle(PurgeContactsCommand request, CancellationToken cancellationToken)
    {
        _context.Contacts.RemoveRange(_context.Contacts);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
