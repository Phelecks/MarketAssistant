using System.ComponentModel.DataAnnotations;
using BaseApplication.Exceptions;
using Informing.Application.Interfaces;
using Informing.Domain.Events.Contact;
using MediatR.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR.Helpers;

namespace Informing.Application.Contact.Commands.UpdateContactByUserId;

public record UpdateContactByUserIdCommand([property : Required]string userId, string? emailAddress, string? phoneNumber, string? fullname) : IRequest<Unit>;

public class UpdateContactByUserIdCommandHandler : IRequestHandler<UpdateContactByUserIdCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateContactByUserIdCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    //public async Task<Unit> HandleAsync(UpdateContactByUserIdCommand request, CancellationToken cancellationToken)
    //{
    //    var entity = await _context.contacts
    //        .SingleAsync(exp => exp.userId.Equals(request.userId), cancellationToken);

    //    if (entity == null)
    //        throw new NotFoundException(nameof(Domain.Entities.Contact), request.userId);

    //    entity.phoneNumber = request.phoneNumber;
    //    entity.emailAddress = request.emailAddress;
    //    entity.fullName = request.fullname;

    //    entity.AddDomainNotification(new ContactUpdatedEvent(entity));

    //    await _context.SaveChangesAsync(cancellationToken);

    //    return Unit.Value;
    //}
    public async Task<Unit> HandleAsync(UpdateContactByUserIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Contacts
            .SingleAsync(exp => exp.UserId.Equals(request.userId), cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Domain.Entities.Contact), request.userId);

        entity.PhoneNumber = request.phoneNumber;
        entity.EmailAddress = request.emailAddress;
        entity.Fullname = request.fullname;

        entity.AddDomainNotification(new ContactUpdatedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
