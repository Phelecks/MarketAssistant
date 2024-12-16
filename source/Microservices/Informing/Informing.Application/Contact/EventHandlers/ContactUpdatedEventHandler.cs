using Informing.Domain.Events.Contact;
using LoggerService.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Informing.Application.Contact.EventHandlers;

public class ContactUpdatedEventHandler : INotificationHandler<ContactUpdatedEvent>
{
    private readonly ILogger<ContactUpdatedEventHandler> _logger;

    public ContactUpdatedEventHandler(ILogger<ContactUpdatedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(ContactUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.Informing, eventName: "Domain Item Updated"),
           "Informing domain event, {@object} updated to {@item}.",
           notification.GetType().Name, notification.item), cancellationToken);

        //Todo: Send email or sms to end user
        

        //return Task.CompletedTask;
    }
}
