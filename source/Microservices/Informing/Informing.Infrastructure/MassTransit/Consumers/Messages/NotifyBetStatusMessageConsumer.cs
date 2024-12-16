using Informing.Application.Interfaces;
using MassTransit;
using MassTransitManager.Messages.Interfaces;

namespace Informing.Infrastructure.MassTransit.Consumers.Messages;

public class NotifyBetStatusMessageConsumer : IConsumer<INotifyBetStatusMessage>
{
    private readonly IGameHubProxy _gameHubProxy;
    private readonly IDiscordService _discordService;

    public NotifyBetStatusMessageConsumer(IGameHubProxy gameHubProxy, IDiscordService discordService)
    {
        _gameHubProxy = gameHubProxy;
        _discordService = discordService;
    }
    public async Task Consume(ConsumeContext<INotifyBetStatusMessage> context)
    {
        var status = (Domain.SignalREntities.BetStatusDto.BetStatus)context.Message.Status;

        await _gameHubProxy.NotifyBetStatusAsync(context.Message.UserId, 
            dto: new Domain.SignalREntities.BetStatusDto(
                game: context.Message.Game,
                matchId: context.Message.MatchId,
                id: context.Message.Id,
                dateTime: context.Message.DateTime,
                externalTokenId: context.Message.ExternalTokenId,
                transactionHash: context.Message.TransactionHash,
                value: context.Message.Value,
                status: status,
                userId: context.Message.UserId,
                option: new Domain.SignalREntities.BetStatusDto.OptionDto(
                    id: context.Message.Option.Id,
                    thumbnail: context.Message.Option.Thumbnail,
                    title: context.Message.Option.Title)
                )
            );

        if (context.Message.DiscordMessage is not null)
        {
            switch (status)
            {
                case Domain.SignalREntities.BetStatusDto.BetStatus.Win:
                    await _discordService.SendWinStatusUpdateMessageAsync(context.Message.DiscordMessage.ChannelId, context.Message.DiscordMessage.Game, context.Message.DiscordMessage.MatchId, context.Message.DiscordMessage.UserWalletAddress, context.Message.DiscordMessage.UserWalletAddressUrl, context.Message.DiscordMessage.FormattedValue, context.Message.DiscordMessage.TransactionHash, context.Message.DiscordMessage.TransactionUrl, context.Message.DiscordMessage.OptionTitle, context.Message.DiscordMessage.OptionThumbnailUrl, context.CancellationToken);
                    break;
                case Domain.SignalREntities.BetStatusDto.BetStatus.Lose:
                    await _discordService.SendLoseStatusUpdateMessageAsync(context.Message.DiscordMessage.ChannelId, context.Message.DiscordMessage.Game, context.Message.DiscordMessage.MatchId, context.Message.DiscordMessage.UserWalletAddress, context.Message.DiscordMessage.UserWalletAddressUrl, context.Message.DiscordMessage.FormattedValue, context.Message.DiscordMessage.TransactionHash, context.Message.DiscordMessage.TransactionUrl, context.Message.DiscordMessage.OptionTitle, context.Message.DiscordMessage.OptionThumbnailUrl, context.CancellationToken);
                    break;
                case Domain.SignalREntities.BetStatusDto.BetStatus.Draw:
                    await _discordService.SendDrawStatusUpdateMessageAsync(context.Message.DiscordMessage.ChannelId, context.Message.DiscordMessage.Game, context.Message.DiscordMessage.MatchId, context.Message.DiscordMessage.UserWalletAddress, context.Message.DiscordMessage.UserWalletAddressUrl, context.Message.DiscordMessage.FormattedValue, context.Message.DiscordMessage.TransactionHash, context.Message.DiscordMessage.TransactionUrl, context.Message.DiscordMessage.OptionTitle, context.Message.DiscordMessage.OptionThumbnailUrl, context.CancellationToken);
                    break;
                default:
                    break;
            }
        }
    }
}