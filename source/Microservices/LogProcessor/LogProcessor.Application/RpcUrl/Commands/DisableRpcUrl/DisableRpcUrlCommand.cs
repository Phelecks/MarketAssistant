using System.ComponentModel.DataAnnotations;
using LogProcessor.Application.Interfaces;
using MediatR.Interfaces;
using MediatR.Helpers;

namespace LogProcessor.Application.RpcUrl.Commands.DisableRpcUrl;

public record DisableRpcUrlCommand([property: Required] long Id, [property: Required] string ErrorMessage) : IRequest<Unit>;

public class Handler(IApplicationDbContext context) : IRequestHandler<DisableRpcUrlCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Unit> HandleAsync(DisableRpcUrlCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.RpcUrls.FindAsync(request.Id, cancellationToken);
        if(entity is null) return Unit.Value;

        entity.Enabled = false;
        entity.ErrorMessage = request.ErrorMessage;

        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}