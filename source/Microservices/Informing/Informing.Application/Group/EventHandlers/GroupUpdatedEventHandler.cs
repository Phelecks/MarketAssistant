using Informing.Domain.Events.Group;
using LoggerService.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Informing.Application.Group.EventHandlers;

public class GroupUpdatedEventHandler : INotificationHandler<GroupUpdatedEvent>
{
    private readonly ILogger<GroupUpdatedEventHandler> _logger;

    public GroupUpdatedEventHandler(ILogger<GroupUpdatedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(GroupUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.Informing, eventName: "Domain Item Updated"),
           "Informing domain event, {@object} updated to {@item}.",
           notification.GetType().Name, notification.item), cancellationToken);

        //Todo: Send email or sms to end user


        //return Task.CompletedTask;
    }
}
