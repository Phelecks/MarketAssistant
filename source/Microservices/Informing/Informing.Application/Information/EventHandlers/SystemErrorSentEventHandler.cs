using Informing.Application.Interfaces;
using Informing.Domain.Events.Information;
using LoggerService.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Informing.Application.Information.EventHandlers;

public class SystemErrorSentEventHandler : INotificationHandler<SystemErrorSentEvent>
{
    private readonly ILogger<SystemErrorSentEventHandler> _logger;
    private readonly IMailService _mailService;

    public SystemErrorSentEventHandler(ILogger<SystemErrorSentEventHandler> logger, IMailService mailService)
    {
        _logger = logger;
        _mailService = mailService;
    }

    public async Task Handle(SystemErrorSentEvent notification, CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.Informing, eventName: "System error created"),
           "Informing domain event, {@object} created: {@item}.",
           notification.GetType().Name, notification.item), cancellationToken);

        foreach (var contactInformation in notification.item.contactInformations)
        {
            if(!string.IsNullOrEmpty(contactInformation.contact.emailAddress))
                await _mailService.SendEmailAsync(notification.item.title, notification.item.content, contactInformation.contact.emailAddress, cancellationToken);
        }
        
    }
}
