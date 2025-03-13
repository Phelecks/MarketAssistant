using BlockChainIdentity.Domain.Events.Wallet;
using LoggerService.Helpers;
using MassTransitManager.Helpers;
using MassTransitManager.Messages;
using MassTransitManager.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BlockChainIdentity.Application.Wallet.EventHandlers;

public class WalletCreatedEventHandler : INotificationHandler<WalletCreatedEvent>
{
    private readonly ILogger<WalletCreatedEventHandler> _logger;
    private readonly IMassTransitService _massTransitService;

    public WalletCreatedEventHandler(ILogger<WalletCreatedEventHandler> logger, IMassTransitService massTransitService)
    {
        _logger = logger;
        _massTransitService = massTransitService;
    }

    public async Task Handle(WalletCreatedEvent notification, CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.Informing, eventName: "Domain Item Created"),
           "BlockChainIdentity domain event, {@object} created with {@item}.",
           notification.GetType().Name, notification.Item), cancellationToken);

        await _massTransitService.SendAsync(
            new CreateUserMessage(notification.Item.Address, notification.Item.Address, string.Empty, string.Empty, notification.ClientId, null),
            Queues.CreateUserMessageQueueName);
    }
}
