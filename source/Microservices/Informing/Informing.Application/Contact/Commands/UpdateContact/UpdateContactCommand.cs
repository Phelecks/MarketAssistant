using BaseApplication.Exceptions;
using BaseApplication.Security;
using Informing.Application.Interfaces;
using Informing.Domain.Events.Contact;
using MediatR.Interfaces;
using System.ComponentModel.DataAnnotations;
using MediatR.Helpers;

namespace Informing.Application.Contact.Commands.UpdateContact;

[Authorize(roles = "Administrators")]
public record UpdateContactCommand([property : Required]long id, string? emailAddress, string? phoneNumber, string? fullname) : IRequest<Unit>;

public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateContactCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    //public async Task<Unit> HandleAsync(UpdateContactCommand request, CancellationToken cancellationToken)
    //{
    //    var entity = await _context.contacts
    //        .FindAsync(new object[] { request.id }, cancellationToken);

    //    if (entity == null)
    //        throw new NotFoundException(nameof(Domain.Entities.Contact), request.id);

    //    entity.phoneNumber = request.phoneNumber;
    //    entity.emailAddress = request.emailAddress;
    //    entity.fullName = request.fullname;

    //    entity.AddDomainNotification(new ContactUpdatedEvent(entity));

    //    await _context.SaveChangesAsync(cancellationToken);

    //    return Unit.Value;
    //}
    public async Task<Unit> HandleAsync(UpdateContactCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Contacts
            .FindAsync([request.id], cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Domain.Entities.Contact), request.id);

        entity.PhoneNumber = request.phoneNumber;
        entity.EmailAddress = request.emailAddress;
        entity.Fullname = request.fullname;

        entity.AddDomainNotification(new ContactUpdatedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
