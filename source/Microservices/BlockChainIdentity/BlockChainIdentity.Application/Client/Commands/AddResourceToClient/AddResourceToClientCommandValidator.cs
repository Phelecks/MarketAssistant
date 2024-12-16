using BlockChainIdentity.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.Client.Commands.AddResourceToClient;

public class AddResourceToClientCommandValidator : AbstractValidator<AddResourceToClientCommand>
{
    private readonly IApplicationDbContext _context;

    public AddResourceToClientCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(request => request.Resources)
            .MustAsync(BeResourcesExistsAsync).WithMessage("At least one of resources does not exist in database.");
    }

    async Task<bool> BeResourcesExistsAsync(List<long> resources, CancellationToken cancellationToken)
    {
        var result = await _context.resources.Where(exp => resources.Any(resourceId => resourceId == exp.id)).CountAsync(cancellationToken);
        return result == resources.Count;
    }
}
