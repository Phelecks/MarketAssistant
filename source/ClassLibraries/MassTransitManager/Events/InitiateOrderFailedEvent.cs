using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class InitiateOrderFailedEvent : IInitiateOrderFailedEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public InitiateOrderFailedEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}