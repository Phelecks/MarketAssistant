using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class OrderMarkedAsDeliveredEvent : IOrderMarkedAsDeliveredEvent
{
    public Guid CorrelationId { get; }

    public OrderMarkedAsDeliveredEvent(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
}