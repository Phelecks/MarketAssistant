using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class TokenPriceUpdatedEvent : ITokenPriceUpdatedEvent
{
    public TokenPriceUpdatedEvent(long tokenId, decimal price)
    {
        TokenId = tokenId;
        Price = price;
    }

    public long TokenId { get; }

    public decimal Price { get; }
}