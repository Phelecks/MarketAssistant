using System.ComponentModel.DataAnnotations;
using MediatR.Interfaces;
using IApplicationDbContext = Informing.Application.Interfaces.IApplicationDbContext;

namespace Informing.Application.GroupContacts.Commands.CreateGroupContact;


public record CreateGroupContactCommand([property: Required] long groupId, [property: Required] long contactId) : IRequest<long>;

public class CreateGroupContactCommandHandler : IRequestHandler<CreateGroupContactCommand, long>
{
    private readonly IApplicationDbContext _context;

    public CreateGroupContactCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> HandleAsync(CreateGroupContactCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.GroupContact
        {
            GroupId = request.groupId,
            ContactId = request.contactId
        };

        await _context.GroupContacts.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
