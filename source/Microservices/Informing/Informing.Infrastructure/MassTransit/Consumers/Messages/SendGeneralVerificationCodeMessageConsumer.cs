using Informing.Application.Information.Commands.SendVerificationCode;
using MassTransit;
using MassTransitManager.Messages.Interfaces;
using MediatR.Interfaces;

namespace Informing.Infrastructure.MassTransit.Consumers.Messages;

public class SendGeneralVerificationCodeMessageConsumer : IConsumer<ISendGeneralVerificationCodeMessage>
{
    private readonly IRequestDispatcher _dispatcher;

    public SendGeneralVerificationCodeMessageConsumer(IRequestDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }
    public async Task Consume(ConsumeContext<ISendGeneralVerificationCodeMessage> context)
    {
        if (context.Message.SendType == BaseDomain.Enums.InformingEnums.InformingSendType.Email)
            await _dispatcher.SendAsync(new SendVerificationCodeCommand(context.Message.UserId, context.Message.SendType, context.Message.VerificationCode), context.CancellationToken);
    }
}