using Informing.Application.Interfaces;
using MassTransit;
using MassTransitManager.Messages.Interfaces;

namespace Informing.Infrastructure.MassTransit.Consumers.Messages;

public class SubmitSystemErrorMessageConsumer : IConsumer<ISubmitSystemErrorMessage>
{
    private readonly IDiscordService _discordService;

    public SubmitSystemErrorMessageConsumer(IDiscordService discordService)
    {
        _discordService = discordService;
    }
    public async Task Consume(ConsumeContext<ISubmitSystemErrorMessage> context)
    {
        await _discordService.SendMessageAsync(context.Message.Content, 1292911958881734756, context.CancellationToken);
    }
}