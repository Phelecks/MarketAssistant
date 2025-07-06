using CustomMediatR.Interfaces;
using LoggerService.Helpers;

namespace SampleApi.Ping.NotificationHandlers.PingCreated;

public class PingCreatedNotificationHandler(ILogger<PingCreatedNotificationHandler> logger) : INotificationHandler<PingCreated>
{
    private readonly ILogger<PingCreatedNotificationHandler> _logger = logger;
    public async Task HandleAsync(PingCreated notification, CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
            eventId: EventTool.GetEventInformation(eventType: EventType.Information, eventName: "Notification Raised"),
            "Notification raised for: {@request}", notification), cancellationToken);
        await Task.CompletedTask;
    }
}