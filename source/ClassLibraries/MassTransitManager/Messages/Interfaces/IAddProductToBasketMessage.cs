namespace MassTransitManager.Messages.Interfaces;

public interface IAddProductToBasketMessage
{
    string UserId { get; }
    long ProductId { get; }
    string Title { get; }
    string? Description { get; }
    decimal Price { get; }
    decimal Discount { get; }
    Uri? Uri { get; }
}