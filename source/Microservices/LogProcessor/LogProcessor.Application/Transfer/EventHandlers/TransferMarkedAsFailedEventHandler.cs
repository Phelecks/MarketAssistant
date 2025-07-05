using LogProcessor.Domain.Events.Transfer;
using LoggerService.Helpers;
using MassTransitManager.Helpers;
using MassTransitManager.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LogProcessor.Application.Transfer.EventHandlers;

public class TransferMarkedAsFailedEventHandler : INotificationHandler<TransferMarkedAsFailedEvent>
{
    private readonly ILogger<TransferMarkedAsFailedEventHandler> _logger;
    private readonly IMassTransitService _massTransitService;

    public TransferMarkedAsFailedEventHandler(ILogger<TransferMarkedAsFailedEventHandler> logger, IMassTransitService massTransitService)
    {
        _logger = logger;
        _massTransitService = massTransitService;
    }

    public async Task Handle(TransferMarkedAsFailedEvent notification, CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.Information, eventName: "Domain Item Updated"),
           "BlockChain Payment domain event, payment {@Hash} failed in chain: {@Chain}.",
            notification.Entity.Hash, notification.Entity.Chain), cancellationToken);

        await _massTransitService.SendAsync<MassTransitManager.Events.Interfaces.ITransferFailedEvent>(
                    new MassTransitManager.Events.TransferFailedEvent(notification.Entity.Id, notification.ErrorMessage),
                    Queues.TransferFailedMessageQueueName,
                    cancellationToken);
    }
}
