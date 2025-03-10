using Informing.Application.Interfaces;
using MassTransit;
using MassTransitManager.Messages.Interfaces;

namespace Informing.Infrastructure.MassTransit.Consumers.Messages;

public class NotifyMatchOverviewMessageConsumer : IConsumer<INotifyMatchOverviewMessage>
{
    private readonly IDiscordService _discordService;

    public NotifyMatchOverviewMessageConsumer(IDiscordService discordService)
    {
        _discordService = discordService;
    }
    public async Task Consume(ConsumeContext<INotifyMatchOverviewMessage> context)
    {
        if (context.Message.DiscordMessage is not null)
        {
            await _discordService.SendMatchOverviewMessageAsync(context.Message.DiscordMessage.ChannelId, context.Message.Game, 
                context.Message.MatchId, context.Message.MatchWalletAddress, context.Message.MatchWalletAddressUrl,
                context.Message.MinutesLeft, context.Message.ImageUrl, context.Message.Bets, context.Message.FormattedBetValue, context.CancellationToken);
        }
    }
}