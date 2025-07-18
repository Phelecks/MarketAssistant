using BaseApplication.Exceptions;
using BaseApplication.Security;
using Informing.Application.Interfaces;
using Informing.Domain.Events.Template;
using MediatR.Helpers;
using MediatR.Interfaces;

namespace Informing.Application.Template.Commands.UpdateTemplate;

[Authorize(roles = "Administrators")]
public record UpdateTemplateCommand : IRequest<Unit>
{
    public UpdateTemplateCommand(long id, string title, string content)
    {
        this.id = id;
        this.title = title;
        this.content = content;
    }

    public long id { get; }

    /// <summary>
    /// Title
    /// </summary>
    public string title { get; }

    /// <summary>
    /// Content
    /// </summary>
    public string content { get; }
}

public class Handler(IApplicationDbContext context) : IRequestHandler<UpdateTemplateCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Unit> HandleAsync(UpdateTemplateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Templates
            .FindAsync(new object[] { request.id }, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Domain.Entities.Template), request.id);

        entity.Content = request.content;
        entity.Title = request.title;

        entity.AddDomainNotification(new TemplateUpdatedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
