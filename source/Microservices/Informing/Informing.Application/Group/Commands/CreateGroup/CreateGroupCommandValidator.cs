using FluentValidation;
using Informing.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Informing.Application.Group.Commands.CreateGroup;

public class CreateGroupCommandValidator : AbstractValidator<CreateGroupCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateGroupCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.title)
            .MustAsync(BeUniqueTitleAsync).WithMessage("The specified title already exists.");
    }

    public async Task<bool> BeUniqueTitleAsync(string title, CancellationToken cancellationToken)
    {
        return await _context.groups
            .AnyAsync(exp => exp.title.Equals(title), cancellationToken);
    }
}
