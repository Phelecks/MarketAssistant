using BlockChainIdentity.Domain.Events.Wallet;
using LoggerService.Helpers;
using MassTransitManager.Helpers;
using MassTransitManager.Messages;
using MassTransitManager.Services;
using MediatR.Interfaces;
using Microsoft.Extensions.Logging;

namespace BlockChainIdentity.Application.Wallet.EventHandlers;

public class WalletCreatedEventHandler(ILogger<WalletCreatedEventHandler> logger, IMassTransitService massTransitService) : INotificationHandler<WalletCreatedEvent>
{
    private readonly ILogger<WalletCreatedEventHandler> _logger = logger;
    private readonly IMassTransitService _massTransitService = massTransitService;

    public async Task HandleAsync(WalletCreatedEvent notification, CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.Information, eventName: "Domain Item Created"),
           "BlockChainIdentity domain event, {@object} created with {@item}.",
           notification.GetType().Name, notification.Item), cancellationToken);

        await _massTransitService.SendAsync(
            new CreateUserMessage(notification.Item.Address, notification.Item.Address, string.Empty, string.Empty, notification.ClientId, null),
            Queues.CreateUserMessageQueueName, cancellationToken);
    }
}
