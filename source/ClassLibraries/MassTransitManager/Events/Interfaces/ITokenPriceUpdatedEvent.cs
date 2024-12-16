namespace MassTransitManager.Events.Interfaces;

public interface ITokenPriceUpdatedEvent
{
    long TokenId { get; }
    decimal Price { get; }
}