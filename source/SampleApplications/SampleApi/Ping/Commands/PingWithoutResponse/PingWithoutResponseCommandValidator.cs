using FluentValidation;

namespace SampleApi.Ping.Commands.PingWithoutResponse;

public class PingCommandValidator: AbstractValidator<PingWithoutResponseCommand>
{
    public PingCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Message must not be empty")
            .NotNull().WithMessage("Message must not be null");
    }
}