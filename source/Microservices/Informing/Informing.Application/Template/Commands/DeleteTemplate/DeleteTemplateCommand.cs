using BaseApplication.Exceptions;
using BaseApplication.Security;
using Informing.Application.Interfaces;
using MediatR.Interfaces;
using MediatR.Helpers;

namespace Informing.Application.Template.Commands.DeleteTemplate;

[Authorize(roles = "Administrators")]
public record DeleteTemplateCommand(int id) : IRequest<Unit>;

public class DeleteTemplateCommandHandler : IRequestHandler<DeleteTemplateCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteTemplateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    //public async Task<Unit> HandleAsync(DeleteTemplateCommand request, CancellationToken cancellationToken)
    //{
    //    var entity = await _context.templates
    //        .FindAsync(new object[] { request.id }, cancellationToken);

    //    if (entity == null)
    //    {
    //        throw new NotFoundException(nameof(Domain.Entities.Template), request.id);
    //    }

    //    _context.templates.Remove(entity);

    //    await _context.SaveChangesAsync(cancellationToken);

    //    return Unit.Value;
    //}
    public async Task<Unit> HandleAsync(DeleteTemplateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Templates
            .FindAsync(new object[] { request.id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Template), request.id);
        }

        _context.Templates.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
