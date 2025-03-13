using BaseApplication.Exceptions;
using BaseApplication.Security;
using Informing.Application.Interfaces;
using Informing.Domain.Events.Information;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Informing.Application.Information.Commands.UpdateInformation;

[Authorize(roles = "Administrators")]
public record UpdateInformationCommand([property : Required]long id, string title, string content) : IRequest<Unit>;

public class UpdateContactCommandHandler : IRequestHandler<UpdateInformationCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateContactCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    //public async Task<Unit> Handle(UpdateInformationCommand request, CancellationToken cancellationToken)
    //{
    //    var entity = await _context.information
    //        .FindAsync(new object[] { request.id }, cancellationToken);

    //    if (entity == null)
    //        throw new NotFoundException(nameof(Domain.Entities.Information), request.id);

    //    entity.title = request.title;
    //    entity.content = request.content;

    //    entity.AddDomainEvent(new InformationUpdatedEvent(entity));

    //    await _context.SaveChangesAsync(cancellationToken);

    //    return Unit.Value;
    //}
    public async Task<Unit> Handle(UpdateInformationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Information
            .FindAsync(new object[] { request.id }, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Domain.Entities.Information), request.id);

        entity.Title = request.title;
        entity.Content = request.content;

        entity.AddDomainEvent(new InformationUpdatedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
