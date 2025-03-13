using FluentValidation;
using Informing.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Informing.Application.Contact.Commands.UpdateContactByUserId;

public class UpdateContactByUserIdValidator : AbstractValidator<UpdateContactByUserIdCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateContactByUserIdValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.userId)
            .NotEmpty().WithMessage("Value is required.")
            .MaximumLength(200).WithMessage("Value must not exceed 200 characters.")
            .MustAsync(BeExistsAsync);
    }

    private async Task<bool> BeExistsAsync(string userId, CancellationToken cancellationToken)
    {
        return await _context.Contacts
            .AnyAsync(exp => exp.UserId.Equals(userId), cancellationToken);
    }
}
