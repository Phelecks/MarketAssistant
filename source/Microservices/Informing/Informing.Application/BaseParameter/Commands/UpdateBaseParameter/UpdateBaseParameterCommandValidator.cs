using FluentValidation;
using Informing.Application.Interfaces;

namespace Informing.Application.BaseParameter.Commands.UpdateBaseParameter;

public class UpdateBaseParameterCommandValidator : AbstractValidator<UpdateBaseParameterCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateBaseParameterCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.value)
            .NotEmpty().WithMessage("Value is required.")
            .MaximumLength(200).WithMessage("Value must not exceed 200 characters.");
    }
}
