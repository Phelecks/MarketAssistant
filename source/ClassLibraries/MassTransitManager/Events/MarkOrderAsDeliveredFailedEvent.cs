using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class MarkOrderAsDeliveredFailedEvent : IMarkOrderAsDeliveredFailedEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public MarkOrderAsDeliveredFailedEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}