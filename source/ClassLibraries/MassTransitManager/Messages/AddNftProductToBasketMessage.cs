using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class AddNftToBasketMessage : IAddNftToBasketMessage
{
    public Guid CorrelationId { get; }
    public string UserId { get; }
    public long ProductId { get; }
    public string Title { get; }
    public string? Description { get; }
    public decimal Price { get; }
    public decimal Discount { get; }
    public Uri? Uri { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    public AddNftToBasketMessage(Guid correlationId, string userId, long productId, string title, string? description, decimal price, decimal discount, Uri? uri)
    {
        CorrelationId = correlationId;
        ProductId = productId;
        UserId = userId;
        Title = title;
        Description = description;
        Price = price;
        Discount = discount;
        Uri = uri;
    }
}