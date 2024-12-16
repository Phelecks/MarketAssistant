using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class OrderMarkedAsRefundedEvent : IOrderMarkedAsRefundedEvent
{
    public Guid CorrelationId { get; }

    public OrderMarkedAsRefundedEvent(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
}