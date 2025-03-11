using FluentValidation;

namespace Informing.Application.Template.Commands.UpdateTemplate;

public class UpdateTemplateCommandValidator : AbstractValidator<UpdateTemplateCommand>
{
        public UpdateTemplateCommandValidator()
    {
        RuleFor(v => v.content)
            .NotEmpty().WithMessage("Value is required.");
    }
}
