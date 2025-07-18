using System.ComponentModel.DataAnnotations;
using LogProcessor.Application.Interfaces;
using MediatR.Interfaces;
using MediatR.Helpers;

namespace LogProcessor.Application.RpcUrl.Commands.EnableRpcUrl;

public record EnableRpcUrlCommand([property: Required] long Id) : IRequest<Unit>;

public class Handler(IApplicationDbContext context) : IRequestHandler<EnableRpcUrlCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Unit> HandleAsync(EnableRpcUrlCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.RpcUrls.FindAsync([request.Id], cancellationToken: cancellationToken);
        if(entity is null) return Unit.Value;

        entity.Enabled = false;
        entity.ErrorMessage = null;

        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}