using Informing.Application.Information.Commands.SendVerificationCodeByEmail;
using MassTransit;
using MassTransitManager.Messages.Interfaces;
using MediatR;

namespace Informing.Infrastructure.MassTransit.Consumers.Messages;

public class SendSignUpVerificationCodeMessageConsumer : IConsumer<ISendSignUpVerificationCodeMessage>
{
    private readonly ISender _sender;

    public SendSignUpVerificationCodeMessageConsumer(ISender sender)
    {
        _sender = sender;
    }
    public async Task Consume(ConsumeContext<ISendSignUpVerificationCodeMessage> context)
    {
        if (context.Message.SendType == BaseDomain.Enums.InformingEnums.InformingSendType.Email)
            await _sender.Send(new SendVerificationCodeByEmailCommand(context.Message.UserId,
                context.Message.Destination, context.Message.VerificationCode));
    }
}