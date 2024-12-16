using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class ProductUpdatedEvent : IProductUpdatedEvent
{
    public long ProductId { get; }

    public decimal NewPrice { get; }

    public string Title { get; }

    public string? Description { get; }

    public decimal Discount { get; }

    public Uri? Uri { get; }

    public ProductUpdatedEvent(long productId, string title, string? description, decimal newPrice, decimal discount, Uri? uri)
    {
        ProductId = productId;
        NewPrice = newPrice;
        Title = title;
        Description = description;
        Discount = discount;
        Uri = uri;
    }
}