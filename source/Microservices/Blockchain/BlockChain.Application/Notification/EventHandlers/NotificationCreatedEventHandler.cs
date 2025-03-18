using BlockChain.Domain.Events.Notification;
using LoggerService.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;
using MassTransitManager.Services;
using MassTransitManager.Helpers;

namespace BlockChain.Application.Notification.EventHandlers;

public class NotificationCreatedEventHandler : INotificationHandler<NotificationCreatedEvent>
{
    private readonly ILogger<NotificationCreatedEventHandler> _logger;
    private readonly IMassTransitService _massTransitService;

    public NotificationCreatedEventHandler(ILogger<NotificationCreatedEventHandler> logger, IMassTransitService massTransitService)
    {
        _logger = logger;
        _massTransitService = massTransitService;
    }

    public async Task Handle(NotificationCreatedEvent notification, CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.BlockChain, eventName: "Domain Item Created"),
           "BlockProcessor domain event, payment hash: {@Hash} initiated in chain: {@Chain}.",
            notification.WalletAddress, notification.Chain), cancellationToken);

        await _massTransitService.SendAsync<MassTransitManager.Messages.Interfaces.INotifyTransferConfirmedMessage>(
                    new MassTransitManager.Messages.NotifyTransferConfirmedMessage(
                        correlationId: notification.CorrelationId, 
                        userId: notification.WalletAddress, 
                        transfer: new MassTransitManager.Messages.NotifyTransferConfirmedMessage.Transfer(
                            Chain: notification.Chain, 
                            Hash: notification.Hash, 
                            From: notification.From, 
                            To: notification.To, 
                            Value: notification.Value, 
                            DateTime: notification.DateTime,
                            Erc20Transfers: notification.Erc20Transfers?.Select(erc20Transfer => new MassTransitManager.Events.Interfaces.ITransferConfirmedEvent.Erc20Transfer(erc20Transfer.From, erc20Transfer.To, erc20Transfer.Value, erc20Transfer.ContractAddress)).ToList(), 
                            Erc721Transfers: notification.Erc721Transfers?.Select(erc721Transfer => new MassTransitManager.Events.Interfaces.ITransferConfirmedEvent.Erc721Transfer(erc721Transfer.From, erc721Transfer.To, erc721Transfer.TokenId, erc721Transfer.ContractAddress)).ToList()
                            )),
                        Queues.NotifyTransferConfirmedMessageQueueName,                            
                        cancellationToken);
    }
}
