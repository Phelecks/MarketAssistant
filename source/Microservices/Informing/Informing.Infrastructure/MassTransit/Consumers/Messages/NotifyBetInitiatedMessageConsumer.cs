using Informing.Application.Interfaces;
using MassTransit;
using MassTransitManager.Messages.Interfaces;

namespace Informing.Infrastructure.MassTransit.Consumers.Messages;

public class NotifyBetInitiatedMessageConsumer : IConsumer<INotifyBetInitiatedMessage>
{
    private readonly IGameHubProxy _gameHubProxy;

    public NotifyBetInitiatedMessageConsumer(IGameHubProxy gameHubProxy)
    {
        _gameHubProxy = gameHubProxy;
    }
    public async Task Consume(ConsumeContext<INotifyBetInitiatedMessage> context)
    {
        await _gameHubProxy.NotifyBetInitiatedAsync(context.Message.UserId,
            dto: new Domain.SignalREntities.BetInitiatedDto(game: context.Message.Game, matchId: context.Message.MatchId,
                id: context.Message.Id, value: context.Message.Value, externalTokenId: context.Message.ExternalTokenId,
                transactionHash: context.Message.TransactionHash, dateTime: context.Message.DateTime,
                option: new Domain.SignalREntities.BetInitiatedDto.OptionDto(id: context.Message.Option.Id, title: context.Message.Option.Title, thumbnail: context.Message.Option.Thumbnail))
        );
    }
}