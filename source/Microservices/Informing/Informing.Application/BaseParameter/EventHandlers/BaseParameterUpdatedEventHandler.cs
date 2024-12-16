using Informing.Domain.Events.BaseParameter;
using LoggerService.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Informing.Application.BaseParameter.EventHandlers;

public class BaseParameterUpdatedEventHandler : INotificationHandler<BaseParameterUpdatedEvent>
{
    private readonly ILogger<BaseParameterUpdatedEventHandler> _logger;

    public BaseParameterUpdatedEventHandler(ILogger<BaseParameterUpdatedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(BaseParameterUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.Informing, eventName: "Domain Item Updated"),
           "Informing domain event, {@object} updated to {@item}.",
           notification.GetType().Name, notification.item), cancellationToken);

        //Todo: Send email or sms to end user


        //return Task.CompletedTask;
    }
}
