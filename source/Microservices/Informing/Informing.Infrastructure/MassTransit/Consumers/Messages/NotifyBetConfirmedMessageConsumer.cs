using Informing.Application.Interfaces;
using MassTransit;
using MassTransitManager.Messages.Interfaces;

namespace Informing.Infrastructure.MassTransit.Consumers.Messages;

public class NotifyBetConfirmedMessageConsumer : IConsumer<INotifyBetConfirmedMessage>
{
    private readonly IGameHubProxy _gameHubProxy;
    private readonly IDiscordService _discordService;

    public NotifyBetConfirmedMessageConsumer(IGameHubProxy gameHubProxy, IDiscordService discordService)
    {
        _gameHubProxy = gameHubProxy;
        _discordService = discordService;
    }
    public async Task Consume(ConsumeContext<INotifyBetConfirmedMessage> context)
    {
        await _gameHubProxy.NotifyBetConfirmedAsync(context.Message.UserId,
            dto: new Domain.SignalREntities.BetConfirmedDto(game: context.Message.Game, matchId: context.Message.MatchId,
                id: context.Message.Id, value: context.Message.Value, externalTokenId: context.Message.ExternalTokenId,
                transactionHash: context.Message.TransactionHash, dateTime: context.Message.DateTime,
                option: new Domain.SignalREntities.BetConfirmedDto.OptionDto(id: context.Message.Option.Id, title: context.Message.Option.Title, thumbnail: context.Message.Option.Thumbnail))
        );

        if (context.Message.DiscordMessage is not null)
            await _discordService.SendBetMessageAsync(context.Message.DiscordMessage.ChannelId, context.Message.DiscordMessage.Game, context.Message.DiscordMessage.MatchId, context.Message.DiscordMessage.UserWalletAddress, context.Message.DiscordMessage.UserWalletAddressUrl, context.Message.DiscordMessage.FormattedValue, context.Message.DiscordMessage.TransactionHash, context.Message.DiscordMessage.TransactionUrl, context.Message.DiscordMessage.OptionTitle, context.Message.DiscordMessage.OptionThumbnailUrl, context.Message.DateTime, context.CancellationToken);
    }
}