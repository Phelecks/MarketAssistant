using Informing.Application.Interfaces;
using MassTransit;
using MassTransitManager.Messages.Interfaces;

namespace Informing.Infrastructure.MassTransit.Consumers.Messages;

public class NotifyMatchMinutesLeftMessageConsumer : IConsumer<INotifyMatchMinutesLeftMessage>
{
    private readonly IGameHubProxy _gameHubProxy;
    private readonly IDiscordService _discordService;

    public NotifyMatchMinutesLeftMessageConsumer(IGameHubProxy gameHubProxy, IDiscordService discordService)
    {
        _gameHubProxy = gameHubProxy;
        _discordService = discordService;
    }
    public async Task Consume(ConsumeContext<INotifyMatchMinutesLeftMessage> context)
    {
        if (context.Message.DiscordMessage is not null)
        {
            await _discordService.SendMatchMinutesLeftMessageAsync(context.Message.DiscordMessage.ChannelId, context.Message.Game, 
                context.Message.MatchId, context.Message.MatchWalletAddress, context.Message.MatchWalletAddressUrl,
                context.Message.MinutesLeft, context.Message.ImageUrl, context.CancellationToken);
        }
    }
}