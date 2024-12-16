using BlockChainIdentity.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.Resource.Commands.CreateResource;

public class CreateResourceCommandValidator : AbstractValidator<CreateResourceCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateResourceCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        //RuleFor(request => request.Title)
        //    .NotEmpty().WithMessage("Title is empty.")
        //    .MustAsync(BeUniqueTitleAsync).WithMessage("Resource already exists.");
    }

    async Task<bool> BeUniqueTitleAsync(string title, CancellationToken cancellationToken)
    {
        return !await _context.resources.AnyAsync(exp => exp.title.Equals(title), cancellationToken);
    }
}
