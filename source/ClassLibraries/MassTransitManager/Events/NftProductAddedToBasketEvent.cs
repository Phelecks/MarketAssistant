using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class NftAddedToBasketEvent : INftAddedToBasketEvent
{
    public Guid CorrelationId { get; }

    public Guid BasketId { get; }

    public NftAddedToBasketEvent(Guid correlationId, Guid basketId)
    {
        CorrelationId = correlationId;
        BasketId = basketId;
    }
}