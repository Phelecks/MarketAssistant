using FluentValidation;
using Informing.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Informing.Application.Contact.Commands.CreateContact;

public class CreateContactCommandValidator : AbstractValidator<CreateContactCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateContactCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.userId)
            .MustAsync(BeUniqueUserIdAsync).WithMessage("The specified userId already exists.");
        RuleFor(v => v.username)
            .MustAsync(BeUniqueUsernameAsync).WithMessage("The specified username already exists.");
    }

    public async Task<bool> BeUniqueUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await _context.Contacts
            .AllAsync(l => l.UserId != userId, cancellationToken);
    }

    public async Task<bool> BeUniqueUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return await _context.Contacts
            .AllAsync(l => l.Username != username, cancellationToken);
    }
}
