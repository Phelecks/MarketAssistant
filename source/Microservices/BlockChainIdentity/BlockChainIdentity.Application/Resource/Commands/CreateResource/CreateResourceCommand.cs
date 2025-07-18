using System.ComponentModel.DataAnnotations;
using BlockChainIdentity.Application.Interfaces;
using MediatR.Helpers;
using MediatR.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.Resource.Commands.CreateResource;

//[Authorize(roles = "Administrators")]
public record CreateResourceCommand([property: Required] string Title) : IRequest<Unit>;

public class Handler(IApplicationDbContext context) : IRequestHandler<CreateResourceCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Unit> HandleAsync(CreateResourceCommand request, CancellationToken cancellationToken)
    {
        if(await _context.Resources.AnyAsync(exp => exp.Title.Equals(request.Title), cancellationToken))
            return Unit.Value;

        var entity = new Domain.Entities.Resource
        {
            Title = request.Title,
        };

        await _context.Resources.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
