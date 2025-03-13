using BaseApplication.Exceptions;
using BaseApplication.Security;
using Informing.Application.Interfaces;
using Informing.Domain.Events.BaseParameter;
using Informing.Domain.Events.Template;
using MediatR;

namespace Informing.Application.Template.Commands.UpdateTemplate;

[Authorize(roles = "Administrators")]
public record UpdateTemplateCommand : IRequest
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

public class Handler : IRequestHandler<UpdateTemplateCommand>
{
    private readonly IApplicationDbContext _context;

    public Handler(IApplicationDbContext context)
    {
        _context = context;
    }

    //public async Task<Unit> Handle(UpdateTemplateCommand request, CancellationToken cancellationToken)
    //{
    //    var entity = await _context.templates
    //        .FindAsync(new object[] { request.id }, cancellationToken);

    //    if (entity == null)
    //        throw new NotFoundException(nameof(Domain.Entities.BaseParameter), request.id);

    //    entity.content = request.content;
    //    entity.title = request.title;

    //    entity.AddDomainEvent(new TemplateUpdatedEvent(entity));

    //    await _context.SaveChangesAsync(cancellationToken);

    //    return Unit.Value;
    //}
    public async Task Handle(UpdateTemplateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Templates
            .FindAsync(new object[] { request.id }, cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Domain.Entities.BaseParameter), request.id);

        entity.Content = request.content;
        entity.Title = request.title;

        entity.AddDomainEvent(new TemplateUpdatedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return;
    }
}
