using BaseApplication.Exceptions;
using BaseApplication.Security;
using Informing.Application.Interfaces;
using MediatR;

namespace Informing.Application.Contact.Commands.DeleteContact;

[Authorize(roles = "Administrators")]
public record DeleteContactCommand(long id) : IRequest<Unit>;

public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteContactCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    //public async Task<Unit> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    //{
    //    var entity = await _context.contacts
    //        .FindAsync(new object[] { request.id }, cancellationToken);

    //    if (entity == null)
    //    {
    //        throw new NotFoundException(nameof(Domain.Entities.Contact), request.id);
    //    }

    //    _context.contacts.Remove(entity);

    //    await _context.SaveChangesAsync(cancellationToken);

    //    return Unit.Value;
    //}
    public async Task<Unit> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Contacts
            .FindAsync([request.id], cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Contact), request.id);
        }

        _context.Contacts.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
