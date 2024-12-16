namespace MassTransitManager.Events.Interfaces;

public interface IProductUpdatedEvent
{
    long ProductId { get; }
    string Title { get; }
    string? Description { get; }
    decimal NewPrice { get; }
    decimal Discount { get; }
    Uri? Uri { get; }
}