using BaseApplication.Exceptions;
using BaseApplication.Security;
using Informing.Application.Interfaces;
using MediatR;

namespace Informing.Application.Information.Commands.DeleteInformation;

[Authorize(roles = "Administrators")]
public record DeleteInformationCommand(long id) : IRequest<Unit>;

public class DeleteInformationCommandHandler : IRequestHandler<DeleteInformationCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteInformationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    //public async Task<Unit> Handle(DeleteInformationCommand request, CancellationToken cancellationToken)
    //{
    //    var entity = await _context.information
    //        .FindAsync(new object[] { request.id }, cancellationToken);

    //    if (entity == null)
    //    {
    //        throw new NotFoundException(nameof(Domain.Entities.Contact), request.id);
    //    }

    //    _context.information.Remove(entity);

    //    await _context.SaveChangesAsync(cancellationToken);

    //    return Unit.Value;
    //}
    public async Task<Unit> Handle(DeleteInformationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.information
            .FindAsync(new object[] { request.id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Contact), request.id);
        }

        _context.information.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
