using System.ComponentModel.DataAnnotations;
using BlockChainIdentity.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.Resource.Commands.CreateResource;

//[Authorize(roles = "Administrators")]
public record CreateResourceCommand([property: Required] string Title) : IRequest<Unit>;

public class Handler : IRequestHandler<CreateResourceCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public Handler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(CreateResourceCommand request, CancellationToken cancellationToken)
    {
        if(await _context.resources.AnyAsync(exp => exp.title.Equals(request.Title), cancellationToken))
            return Unit.Value;

        var entity = new Domain.Entities.Resource
        {
            title = request.Title,
        };

        await _context.resources.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
