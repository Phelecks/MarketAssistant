using System.ComponentModel.DataAnnotations;
using MediatR.Interfaces;
using IApplicationDbContext = Informing.Application.Interfaces.IApplicationDbContext;

namespace Informing.Application.Contact.Commands.CreateContact;


public record CreateContactCommand([property: Required]string userId, string username, string? phoneNumber = null, string? emailAddress = null) : IRequest<long>;

public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, long>
{
    private readonly IApplicationDbContext _context;

    public CreateContactCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> HandleAsync(CreateContactCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Contact
        {
            UserId = request.userId,
            Username = request.username,
            PhoneNumber = request.phoneNumber,
            EmailAddress = request.emailAddress
        };

        await _context.Contacts.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
