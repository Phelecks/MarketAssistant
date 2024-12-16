using FluentValidation;
using Informing.Application.Interfaces;

namespace Informing.Application.Group.Commands.UpdateGroup;

public class UpdateContactCommandValidator : AbstractValidator<UpdateGroupCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateContactCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        //RuleFor(v => v.)
        //    .NotEmpty().WithMessage("Value is required.")
        //    .MaximumLength(200).WithMessage("Value must not exceed 200 characters.");
    }
}
