using FluentValidation;

namespace Informing.Application.GroupContacts.Commands.CreateGroupContact;

public class CreateGroupContactCommandValidator : AbstractValidator<CreateGroupContactCommand>
{
    public CreateGroupContactCommandValidator()
    {
    }
}
