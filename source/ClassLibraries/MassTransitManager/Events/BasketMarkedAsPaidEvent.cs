using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class BasketMarkedAsPaidEvent : IBasketMarkedAsPaidEvent
{
    public Guid CorrelationId { get; }

    public List<IBasketMarkedAsPaidEvent.BasketItem> BasketItems { get; }

    public BasketMarkedAsPaidEvent(Guid correlationId, List<IBasketMarkedAsPaidEvent.BasketItem> basketItems)
    {
        CorrelationId = correlationId;
        BasketItems = basketItems;
    }
}