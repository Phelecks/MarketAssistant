using Informing.Application.Information.Commands.SendVerificationCodeByEmail;
using MassTransit;
using MassTransitManager.Messages.Interfaces;
using MediatR.Interfaces;

namespace Informing.Infrastructure.MassTransit.Consumers.Messages;

public class SendSignUpVerificationCodeMessageConsumer : IConsumer<ISendSignUpVerificationCodeMessage>
{
    private readonly IRequestDispatcher _dispatcher;

    public SendSignUpVerificationCodeMessageConsumer(IRequestDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }
    public async Task Consume(ConsumeContext<ISendSignUpVerificationCodeMessage> context)
    {
        if (context.Message.SendType == BaseDomain.Enums.InformingEnums.InformingSendType.Email)
            await _dispatcher.SendAsync(new SendVerificationCodeByEmailCommand(context.Message.UserId,
                context.Message.Destination, context.Message.VerificationCode), context.CancellationToken);
    }
}