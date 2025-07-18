using System.ComponentModel.DataAnnotations;
using MediatR.Interfaces;
using IApplicationDbContext = Informing.Application.Interfaces.IApplicationDbContext;

namespace Informing.Application.Group.Commands.CreateGroup;


public record CreateGroupCommand([property: Required] string title, string description) : IRequest<long>;

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, long>
{
    private readonly IApplicationDbContext _context;

    public CreateGroupCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> HandleAsync(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Group
        {
            Title = request.title,
            Description = request.description
        };

        _context.Groups.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
