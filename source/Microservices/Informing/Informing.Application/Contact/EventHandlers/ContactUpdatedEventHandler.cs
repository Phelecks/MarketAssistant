﻿using Informing.Domain.Events.Contact;
using LoggerService.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Informing.Application.Contact.EventHandlers;

public class ContactUpdatedEventHandler(ILogger<ContactUpdatedEventHandler> logger) : INotificationHandler<ContactUpdatedEvent>
{
    private readonly ILogger<ContactUpdatedEventHandler> _logger = logger;

    public async Task Handle(ContactUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.Informing, eventName: "Domain Item Updated"),
           "Informing domain event, {@object} updated to {@item}.",
           notification.GetType().Name, notification.item), cancellationToken);
        

        await Task.CompletedTask;
    }
}
