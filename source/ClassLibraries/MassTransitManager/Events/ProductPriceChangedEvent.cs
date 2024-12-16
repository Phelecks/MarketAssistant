using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class ProductPriceChangedEvent : IProductPriceChangedEvent
{
    public long ProductId { get; }

    public decimal NewPrice { get; }

    public ProductPriceChangedEvent(long productId, decimal newPrice)
    {
        ProductId = productId;
        NewPrice = newPrice;
    }
}