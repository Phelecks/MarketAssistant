using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class MarkOrderAsRefundedFailedEvent : IMarkOrderAsRefundedFailedEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public MarkOrderAsRefundedFailedEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}