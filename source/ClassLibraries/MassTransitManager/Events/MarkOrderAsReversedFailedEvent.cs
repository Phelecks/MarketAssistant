using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class MarkOrderAsReversedFailedEvent : IMarkOrderAsReversedFailedEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public MarkOrderAsReversedFailedEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}