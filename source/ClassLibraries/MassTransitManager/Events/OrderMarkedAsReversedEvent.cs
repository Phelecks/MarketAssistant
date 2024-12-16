using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class OrderMarkedAsReversedEvent : IOrderMarkedAsReversedEvent
{
    public Guid CorrelationId { get; }

    public OrderMarkedAsReversedEvent(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
}