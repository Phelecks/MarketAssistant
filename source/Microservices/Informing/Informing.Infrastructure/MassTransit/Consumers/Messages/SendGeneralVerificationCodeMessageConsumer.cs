using Informing.Application.Information.Commands.SendVerificationCode;
using MassTransit;
using MassTransitManager.Messages.Interfaces;
using MediatR;

namespace Informing.Infrastructure.MassTransit.Consumers.Messages;

public class SendGeneralVerificationCodeMessageConsumer : IConsumer<ISendGeneralVerificationCodeMessage>
{
    private readonly ISender _sender;

    public SendGeneralVerificationCodeMessageConsumer(ISender sender)
    {
        _sender = sender;
    }
    public async Task Consume(ConsumeContext<ISendGeneralVerificationCodeMessage> context)
    {
        if (context.Message.SendType == BaseDomain.Enums.InformingEnums.InformingSendType.Email)
            await _sender.Send(new SendVerificationCodeCommand(context.Message.UserId, context.Message.SendType, context.Message.VerificationCode));
    }
}