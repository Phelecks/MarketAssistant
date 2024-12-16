using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class MarkOrderAsPaidFailedEvent : IMarkOrderAsPaidFailedEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public MarkOrderAsPaidFailedEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}