using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class OrderMarkedAsPaidEvent : IOrderMarkedAsPaidEvent
{
    public Guid CorrelationId { get; }

    public OrderMarkedAsPaidEvent(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
}