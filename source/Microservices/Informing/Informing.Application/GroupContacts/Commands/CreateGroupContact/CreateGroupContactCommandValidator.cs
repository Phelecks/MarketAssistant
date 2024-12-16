using FluentValidation;
using Informing.Application.Interfaces;

namespace Informing.Application.GroupContacts.Commands.CreateGroupContact;

public class CreateGroupContactCommandValidator : AbstractValidator<CreateGroupContactCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateGroupContactCommandValidator(IApplicationDbContext context)
    {
        _context = context;
    }
}
