using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class StockRemovedEvent : IStockRemovedEvent
{
    public Guid CorrelationId { get; }

    public StockRemovedEvent(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
}