using FluentValidation;

namespace SampleApi.Ping.Commands;

public class PingCommandValidator: AbstractValidator<PingCommand>
{
    public PingCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Message must not be empty")
            .NotNull().WithMessage("Message must not be null");
    }
}