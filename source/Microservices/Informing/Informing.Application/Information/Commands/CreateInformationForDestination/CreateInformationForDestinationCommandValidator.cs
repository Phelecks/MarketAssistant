using FluentValidation;
using Informing.Application.Interfaces;

namespace Informing.Application.Information.Commands.CreateInformationForDestination;

public class CreateInformationForDestinationCommandValidator : AbstractValidator<CreateInformationForDestinationCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateInformationForDestinationCommandValidator(IApplicationDbContext context)
    {
        _context = context;
    }
}
