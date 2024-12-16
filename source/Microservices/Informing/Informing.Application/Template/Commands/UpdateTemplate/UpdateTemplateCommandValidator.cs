using FluentValidation;
using Informing.Application.Interfaces;

namespace Informing.Application.Template.Commands.UpdateTemplate;

public class UpdateTemplateCommandValidator : AbstractValidator<UpdateTemplateCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTemplateCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.content)
            .NotEmpty().WithMessage("Value is required.");
    }
}
