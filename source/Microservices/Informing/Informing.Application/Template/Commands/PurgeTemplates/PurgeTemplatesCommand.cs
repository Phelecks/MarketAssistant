using BaseApplication.Security;
using Informing.Application.Interfaces;
using MediatR;

namespace Informing.Application.Template.Commands.PurgeTemplates;

[Authorize(roles = "Administrators")]
public record PurgeTemplatesCommand : IRequest<Unit>;

public class PurgeTemplatesCommandHandler : IRequestHandler<PurgeTemplatesCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public PurgeTemplatesCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    //public async Task<Unit> Handle(PurgeTemplatesCommand request, CancellationToken cancellationToken)
    //{
    //    _context.templates.RemoveRange(_context.templates);

    //    await _context.SaveChangesAsync(cancellationToken);

    //    return Unit.Value;
    //}
    public async Task<Unit> Handle(PurgeTemplatesCommand request, CancellationToken cancellationToken)
    {
        _context.Templates.RemoveRange(_context.Templates);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
