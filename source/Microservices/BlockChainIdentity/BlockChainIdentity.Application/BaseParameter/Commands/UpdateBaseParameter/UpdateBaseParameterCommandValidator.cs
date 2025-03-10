using BlockChainIdentity.Application.Interfaces;
using FluentValidation;

namespace BlockChainIdentity.Application.BaseParameter.Commands.UpdateBaseParameter;

public class UpdateBaseParameterCommandValidator : AbstractValidator<UpdateBaseParameterCommand>
{
    public UpdateBaseParameterCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.value)
            .NotEmpty().WithMessage("Value is required.")
            .MaximumLength(200).WithMessage("Value must not exceed 200 characters.");
    }
}
