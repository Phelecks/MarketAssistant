using BaseDomain.Enums;
using FluentValidation;
using Informing.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Informing.Application.Template.Commands.CreateTemplate;

public class CreateTemplateCommandValidator : AbstractValidator<CreateTemplateCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateTemplateCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        When(exp => exp.informingSendType == BaseDomain.Enums.InformingEnums.InformingSendType.Sms, () => 
            RuleFor(v => v.informingType)
                .MustAsync(BeUniqueSmsInformingTypeAsync).WithMessage("The specified record already exists."));

        When(exp => exp.informingSendType == BaseDomain.Enums.InformingEnums.InformingSendType.Email, () =>
            RuleFor(v => v.informingType)
                .MustAsync(BeUniqueEmailInformingTypeAsync).WithMessage("The specified record already exists."));

        When(exp => exp.informingSendType == BaseDomain.Enums.InformingEnums.InformingSendType.Notification, () =>
            RuleFor(v => v.informingType)
                .MustAsync(BeUniqueNotificationInformingTypeAsync).WithMessage("The specified record already exists."));
    }

    private async Task<bool> BeUniqueSmsInformingTypeAsync(BaseDomain.Enums.InformingEnums.InformingType informingType, CancellationToken cancellationToken)
    {
        return !await _context.templates
            .AnyAsync(exp => exp.informingSendType == BaseDomain.Enums.InformingEnums.InformingSendType.Sms && exp.informingType == informingType, cancellationToken);
    }

    private async Task<bool> BeUniqueEmailInformingTypeAsync(BaseDomain.Enums.InformingEnums.InformingType informingType, CancellationToken cancellationToken)
    {
        return !await _context.templates
            .AnyAsync(exp => exp.informingSendType == BaseDomain.Enums.InformingEnums.InformingSendType.Email && exp.informingType == informingType, cancellationToken);
    }

    private async Task<bool> BeUniqueNotificationInformingTypeAsync(BaseDomain.Enums.InformingEnums.InformingType informingType, CancellationToken cancellationToken)
    {
        return !await _context.templates
            .AnyAsync(exp => exp.informingSendType == BaseDomain.Enums.InformingEnums.InformingSendType.Notification && exp.informingType == informingType, cancellationToken);
    }
}
