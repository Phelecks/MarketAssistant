namespace MassTransitManager.Events.Interfaces;

public interface IProductPriceChangedEvent
{
    long ProductId { get; }

    decimal NewPrice { get; }
}