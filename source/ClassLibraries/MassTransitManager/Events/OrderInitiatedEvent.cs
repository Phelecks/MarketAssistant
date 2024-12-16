using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class OrderInitiatedEvent : IOrderInitiatedEvent
{
    public Guid CorrelationId { get; }

    public OrderInitiatedEvent(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
}