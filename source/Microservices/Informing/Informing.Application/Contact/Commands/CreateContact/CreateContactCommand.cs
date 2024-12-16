using System.ComponentModel.DataAnnotations;
using MediatR;
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

    public async Task<long> Handle(CreateContactCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Contact
        {
            userId = request.userId,
            username = request.username,
            phoneNumber = request.phoneNumber,
            emailAddress = request.emailAddress
        };

        await _context.contacts.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.id;
    }
}
