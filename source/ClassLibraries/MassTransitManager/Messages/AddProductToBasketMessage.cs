using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class AddProductToBasketMessage : IAddProductToBasketMessage
{
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
    public AddProductToBasketMessage(string userId, long productId, string title, string? description, decimal price, decimal discount, Uri? uri)
    {
        ProductId = productId;
        UserId = userId;
        Title = title;
        Description = description;
        Price = price;
        Discount = discount;
        Uri = uri;
    }
}