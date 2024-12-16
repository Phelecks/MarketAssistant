using BaseApplication.Exceptions;
using BaseApplication.Security;
using Informing.Application.Interfaces;
using Informing.Domain.Events.Contact;
using MediatR;
using System.ComponentModel.DataAnnotations;

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

    //public async Task<Unit> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
    //{
    //    var entity = await _context.contacts
    //        .FindAsync(new object[] { request.id }, cancellationToken);

    //    if (entity == null)
    //        throw new NotFoundException(nameof(Domain.Entities.Contact), request.id);

    //    entity.phoneNumber = request.phoneNumber;
    //    entity.emailAddress = request.emailAddress;
    //    entity.fullName = request.fullname;

    //    entity.AddDomainEvent(new ContactUpdatedEvent(entity));

    //    await _context.SaveChangesAsync(cancellationToken);

    //    return Unit.Value;
    //}
    public async Task<Unit> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.contacts
            .FindAsync(new object[] { request.id }, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Domain.Entities.Contact), request.id);

        entity.phoneNumber = request.phoneNumber;
        entity.emailAddress = request.emailAddress;
        entity.fullname = request.fullname;

        entity.AddDomainEvent(new ContactUpdatedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
