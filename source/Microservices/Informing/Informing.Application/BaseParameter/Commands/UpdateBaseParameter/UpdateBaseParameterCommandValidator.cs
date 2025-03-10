using FluentValidation;

namespace Informing.Application.BaseParameter.Commands.UpdateBaseParameter;

public class UpdateBaseParameterCommandValidator : AbstractValidator<UpdateBaseParameterCommand>
{
    public UpdateBaseParameterCommandValidator()
    {

        RuleFor(v => v.value)
            .NotEmpty().WithMessage("Value is required.")
            .MaximumLength(200).WithMessage("Value must not exceed 200 characters.");
    }
}
