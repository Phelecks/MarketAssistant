using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class ProductSoldoutEvent : IProductSoldoutEvent
{
    public long ProductId { get; }

    public ProductSoldoutEvent(long productId)
    {
        ProductId = productId;
    }
}