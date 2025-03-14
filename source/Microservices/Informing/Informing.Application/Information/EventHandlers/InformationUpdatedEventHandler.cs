using Informing.Domain.Events.Information;
using LoggerService.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Informing.Application.Information.EventHandlers;

public class InformationUpdatedEventHandler : INotificationHandler<InformationUpdatedEvent>
{
    private readonly ILogger<InformationUpdatedEventHandler> _logger;

    public InformationUpdatedEventHandler(ILogger<InformationUpdatedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(InformationUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.Informing, eventName: "Domain Item Updated"),
           "Informing domain event, {@object} updated to {@item}.",
           notification.GetType().Name, notification.item), cancellationToken);


        await Task.CompletedTask;
    }
}
