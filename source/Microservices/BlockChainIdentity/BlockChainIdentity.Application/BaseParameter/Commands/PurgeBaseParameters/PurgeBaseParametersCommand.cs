using BaseApplication.Security;
using BlockChainIdentity.Application.Interfaces;
using MediatR;

namespace BlockChainIdentity.Application.BaseParameter.Commands.PurgeBaseParameters;

[Authorize(roles = "Administrators")]
public record PurgeBaseParametersCommand : IRequest<Unit>;

public class Handler : IRequestHandler<PurgeBaseParametersCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public Handler(IApplicationDbContext context)
    {
        _context = context;
    }

    //public async Task<Unit> Handle(PurgeBaseParametersCommand request, CancellationToken cancellationToken)
    //{
    //    _context.baseParameters.RemoveRange(_context.baseParameters);

    //    await _context.SaveChangesAsync(cancellationToken);

    //    return Unit.Value;
    //}
    public async Task<Unit> Handle(PurgeBaseParametersCommand request, CancellationToken cancellationToken)
    {
        _context.BaseParameters.RemoveRange(_context.BaseParameters);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
