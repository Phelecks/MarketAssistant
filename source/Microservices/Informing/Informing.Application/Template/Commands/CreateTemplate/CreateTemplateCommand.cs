using BaseApplication.Security;
using MediatR;
using IApplicationDbContext = Informing.Application.Interfaces.IApplicationDbContext;

namespace Informing.Application.Template.Commands.CreateTemplate;


[Authorize(roles = "Administrators")]
public record CreateTemplateCommand(string title, string content, BaseDomain.Enums.InformingEnums.InformingType informingType, BaseDomain.Enums.InformingEnums.InformingSendType informingSendType) : IRequest<long>
{
    /// <summary>
    /// Title
    /// </summary>
    public string title { get; } = title;

    /// <summary>
    /// Content
    /// </summary>
    public string content { get; } = content;

    /// <summary>
    /// Information type
    /// </summary>
    public BaseDomain.Enums.InformingEnums.InformingType informingType { get; } = informingType;

    /// <summary>
    /// Publication type
    /// </summary>
    public BaseDomain.Enums.InformingEnums.InformingSendType informingSendType { get; } = informingSendType;
}

public class CreateTemplateCommandHandler : IRequestHandler<CreateTemplateCommand, long>
{
    private readonly IApplicationDbContext _context;

    public CreateTemplateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Template
        {
            content= request.content,
            informingType=request.informingType,
            informingSendType=request.informingSendType,
            title =request.title,
        };

        await _context.templates.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.id;
    }
}
