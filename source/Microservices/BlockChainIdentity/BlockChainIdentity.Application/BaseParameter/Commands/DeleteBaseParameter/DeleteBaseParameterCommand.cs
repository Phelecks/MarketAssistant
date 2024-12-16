using BaseApplication.Exceptions;
using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using MediatR;

namespace BlockChainIdentity.Application.BaseParameter.Commands.DeleteBaseParameter;

[Authorize(roles = "Administrators")]
public record DeleteBaseParameterCommand(int id) : IRequest<Unit>;

public class Handler : IRequestHandler<DeleteBaseParameterCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public Handler(IApplicationDbContext context)
    {
        _context = context;
    }

    //public async Task<Unit> Handle(DeleteBaseParameterCommand request, CancellationToken cancellationToken)
    //{
    //    var entity = await _context.baseParameters
    //        .FindAsync(new object[] { request.id }, cancellationToken);

    //    if (entity == null)
    //    {
    //        throw new NotFoundException(nameof(Domain.Entities.BaseParameter), request.id);
    //    }

    //    _context.baseParameters.Remove(entity);

    //    await _context.SaveChangesAsync(cancellationToken);

    //    return Unit.Value;
    //}
    public async Task<Unit> Handle(DeleteBaseParameterCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.baseParameters
            .FindAsync(new object[] { request.id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.BaseParameter), request.id);
        }

        _context.baseParameters.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
