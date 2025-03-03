using BlockChainIdentity.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.Client.Commands.CreateClient;

public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateClientCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(request => request.ClientSecret)
            .Length(min: 6, max: 16).WithMessage("Client secret length must between 6 and 16 characters.");

        RuleFor(request => request.ClientId)
            .MustAsync(BeUniqueClientIdAsync).WithMessage("The specified field already exists.");

        RuleFor(request => request.Resources)
            .MustAsync(BeResourcesExistsAsync).WithMessage("At least one of resources does not exist in database.");
    }

    async Task<bool> BeUniqueClientIdAsync(string clientId, CancellationToken cancellationToken)
    {
        return await _context.clients.AllAsync(l => l.clientId != clientId, cancellationToken);
    }

    async Task<bool> BeResourcesExistsAsync(List<long> resources, CancellationToken cancellationToken)
    {
        var result = await _context.resources.Where(exp => resources.Any(resourceId => resourceId == exp.Id)).CountAsync(cancellationToken);
        return result == resources.Count;
    }
}
