using FluentValidation;
using Informing.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Informing.Application.Information.Commands.SendVerificationCodeByEmail;

public class SendVerificationCodeByEmailValidator : AbstractValidator<SendVerificationCodeByEmailCommand>
{
    private readonly IApplicationDbContext _context;

    public SendVerificationCodeByEmailValidator(IApplicationDbContext context)
    {
        _context = context;

        //RuleFor(v => v.userId)
        //    .MustAsync(ExistsUserIdWithEmailAddressAsync).WithMessage("Contact for userId not found or it does not have any email address.");

    }

    //private async Task<bool> ExistsUserIdWithEmailAddressAsync(string userId, CancellationToken cancellationToken)
    //{
    //    return await _context.contacts.AnyAsync(exp => exp.userId.Equals(userId) && !string.IsNullOrEmpty(exp.emailAddress), cancellationToken);
    //}
}
