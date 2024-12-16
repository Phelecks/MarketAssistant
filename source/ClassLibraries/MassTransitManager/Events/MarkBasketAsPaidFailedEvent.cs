using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class MarkBasketAsPaidFailedEvent : IMarkBasketAsPaidFailedEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public MarkBasketAsPaidFailedEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}