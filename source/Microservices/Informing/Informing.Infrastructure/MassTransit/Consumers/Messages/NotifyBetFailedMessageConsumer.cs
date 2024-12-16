using Informing.Application.Interfaces;
using MassTransit;
using MassTransitManager.Messages.Interfaces;

namespace Informing.Infrastructure.MassTransit.Consumers.Messages;

public class NotifyBetFailedMessageConsumer : IConsumer<INotifyBetFailedMessage>
{
    private readonly IGameHubProxy _gameHubProxy;

    public NotifyBetFailedMessageConsumer(IGameHubProxy gameHubProxy)
    {
        _gameHubProxy = gameHubProxy;
    }
    public async Task Consume(ConsumeContext<INotifyBetFailedMessage> context)
    {
        await _gameHubProxy.NotifyBetFailedAsync(context.Message.UserId,
            dto: new Domain.SignalREntities.BetFailedDto(game: context.Message.Game, matchId: context.Message.MatchId,
                id: context.Message.Id, value: context.Message.Value, externalTokenId: context.Message.ExternalTokenId,
                transactionHash: context.Message.TransactionHash, dateTime: context.Message.DateTime,
                option: new Domain.SignalREntities.BetFailedDto.OptionDto(id: context.Message.Option.Id, title: context.Message.Option.Title, thumbnail: context.Message.Option.Thumbnail),
                message: context.Message.Message)
        );
    }
}