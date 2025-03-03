using Informing.Application.Interfaces;
using Informing.Domain.Entities;
using Informing.Domain.Events.Information;
using MediatR;
using System.ComponentModel.DataAnnotations;
using IApplicationDbContext = Informing.Application.Interfaces.IApplicationDbContext;

namespace Informing.Application.Information.Commands.CreateInformationForDestination;


public record CreateInformationForDestinationCommand([property: Required] string title, [property: Required] string content, [property: Required] InformationType type, string destination) : IRequest<long>;

public class CreateInformationCommandHandler : IRequestHandler<CreateInformationForDestinationCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly IContactService _contactService;

    public CreateInformationCommandHandler(IApplicationDbContext context, IContactService contactService)
    {
        _context = context;
        _contactService = contactService;
    }

    public async Task<long> Handle(CreateInformationForDestinationCommand request, CancellationToken cancellationToken)
    {
        var contact = await _contactService.FindContactAsync(request.destination, cancellationToken);
        
        var entity = new Domain.Entities.Information
        {
            content = request.content,
            //destination = request.destination,
            type = request.type,
            title = request.title,
            enabled = true,
            contactInformations = new List<ContactInformation>()
            {
                new ContactInformation
                {
                    contact = contact
                }
            }
        };

        await _context.information.AddAsync(entity, cancellationToken);

        entity.AddDomainEvent(new InformationCreatedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
