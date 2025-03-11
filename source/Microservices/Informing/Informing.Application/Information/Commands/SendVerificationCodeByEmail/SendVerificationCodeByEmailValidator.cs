using FluentValidation;

namespace Informing.Application.Information.Commands.SendVerificationCodeByEmail;

public class SendVerificationCodeByEmailValidator : AbstractValidator<SendVerificationCodeByEmailCommand>
{
    public SendVerificationCodeByEmailValidator()
    {
    }
}
