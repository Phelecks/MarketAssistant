using BaseDomain.Enums;
using FluentValidation;
using Informing.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Informing.Application.Information.Commands.SendError;

public class SendErrorCommandValidator : AbstractValidator<SendErrorCommand>
{
    private readonly IApplicationDbContext _context;

    public SendErrorCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        //When(exp => exp.sendType == InformingEnums.InformingSendType.Email, () =>
        //    RuleFor(v => v.userId)
        //        .MustAsync(ExistsUserIdWithEmailAddressAsync)
        //        .WithMessage("Contact for userId not found or it does not have any email address."));
        //When(exp => exp.sendType == InformingEnums.InformingSendType.Sms, () =>
        //    RuleFor(v => v.userId)
        //        .MustAsync(ExistsUserIdWithPhoneNumberAsync)
        //        .WithMessage("Contact for userId not found or it does not have any phone number."));


    }

    private async Task<bool> ExistsUserIdWithEmailAddressAsync(string userId, CancellationToken cancellationToken)
    {
        return await _context.contacts.AnyAsync(exp => exp.userId.Equals(userId) && !string.IsNullOrEmpty(exp.emailAddress), cancellationToken);
    }
    private async Task<bool> ExistsUserIdWithPhoneNumberAsync(string userId, CancellationToken cancellationToken)
    {
        return await _context.contacts.AnyAsync(exp => exp.userId.Equals(userId) && !string.IsNullOrEmpty(exp.phoneNumber), cancellationToken);
    }
}
