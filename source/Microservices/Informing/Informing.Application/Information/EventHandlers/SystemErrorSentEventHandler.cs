using Informing.Domain.Events.Information;
using LoggerService.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Informing.Application.Information.EventHandlers;

public class SystemErrorSentEventHandler : INotificationHandler<SystemErrorSentEvent>
{
    private readonly ILogger<SystemErrorSentEventHandler> _logger;

    public SystemErrorSentEventHandler(ILogger<SystemErrorSentEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(SystemErrorSentEvent notification, CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.Information, eventName: "System error created"),
           "Informing domain event, {@object} created: {@item}.",
           notification.GetType().Name, notification.item), cancellationToken);
        
        await Task.CompletedTask;
    }
}
