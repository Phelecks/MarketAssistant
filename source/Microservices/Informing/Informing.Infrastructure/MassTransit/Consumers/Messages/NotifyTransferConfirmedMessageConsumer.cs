using Informing.Application.Interfaces;
using MassTransit;
using MassTransitManager.Messages.Interfaces;

namespace Informing.Infrastructure.MassTransit.Consumers.Messages;

public class NotifyTransferConfirmedMessageConsumer : IConsumer<INotifyTransferConfirmedMessage>
{
    private readonly IDiscordService _discordService;

    public NotifyTransferConfirmedMessageConsumer(IDiscordService discordService)
    {
        _discordService = discordService;
    }
    public async Task Consume(ConsumeContext<INotifyTransferConfirmedMessage> context)
    {
        if (context.Message.Discord is not null)
            await _discordService.SendMessageAsync($"New Transfer \n Chain: {context.Message.Chain} \n Hash: {context.Message.Hash} \n From: {context.Message.From} \n To: {context.Message.To}", context.Message.Discord.ChannelId, context.CancellationToken);            
    }
}