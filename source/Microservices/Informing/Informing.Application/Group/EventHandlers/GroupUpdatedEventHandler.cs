using Informing.Domain.Events.Group;
using LoggerService.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Informing.Application.Group.EventHandlers;

public class GroupUpdatedEventHandler(ILogger<GroupUpdatedEventHandler> logger) : INotificationHandler<GroupUpdatedEvent>
{
    private readonly ILogger<GroupUpdatedEventHandler> _logger = logger;

    public async Task Handle(GroupUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.Information, eventName: "Domain Item Updated"),
           "Informing domain event, {@object} updated to {@item}.",
           notification.GetType().Name, notification.item), cancellationToken);


        await Task.CompletedTask;
    }
}
