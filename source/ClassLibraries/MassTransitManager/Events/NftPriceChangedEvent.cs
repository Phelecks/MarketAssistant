using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class NftPriceChangedEvent : INftPriceChangedEvent
{
    public NftPriceChangedEvent(Guid correlationId, long productId, string title, string? description, decimal price, decimal discount, Uri? uri)
    {
        CorrelationId = correlationId;
        ProductId = productId;
        Title = title;
        Description = description;
        Price = price;
        Discount = discount;
        Uri = uri;
    }

    public Guid CorrelationId { get; }

    public long ProductId { get; }

    public string Title { get; }

    public string? Description { get; }

    public decimal Price { get; }

    public decimal Discount { get; }

    public Uri? Uri { get; }
}