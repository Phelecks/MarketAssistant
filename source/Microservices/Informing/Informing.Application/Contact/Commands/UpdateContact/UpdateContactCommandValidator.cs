using FluentValidation;
using Informing.Application.Interfaces;

namespace Informing.Application.Contact.Commands.UpdateContact;

public class UpdateContactCommandValidator : AbstractValidator<UpdateContactCommand>
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
