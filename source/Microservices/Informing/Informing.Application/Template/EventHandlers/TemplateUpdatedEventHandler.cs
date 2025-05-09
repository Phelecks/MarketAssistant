﻿using Informing.Domain.Events.Template;
using LoggerService.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Informing.Application.Template.EventHandlers;

public class TemplateUpdatedEventHandler : INotificationHandler<TemplateUpdatedEvent>
{
    private readonly ILogger<TemplateUpdatedEventHandler> _logger;

    public TemplateUpdatedEventHandler(ILogger<TemplateUpdatedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(TemplateUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.Informing, eventName: "Domain Item Updated"),
           "Informing domain event, {@object} updated to {@item}.",
           notification.GetType().Name, notification.item), cancellationToken);


        await Task.CompletedTask;
    }
}
