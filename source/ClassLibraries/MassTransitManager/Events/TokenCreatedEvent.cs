using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class TokenCreatedEvent : ITokenCreatedEvent
{
    public TokenCreatedEvent(long tokenId, string symbol, BaseDomain.Enums.BlockChainEnums.Chain chain)
    {
        TokenId = tokenId;
        Symbol = symbol;
        Chain = chain;
    }

    public long TokenId { get; }
    public string Symbol { get; }
    public BaseDomain.Enums.BlockChainEnums.Chain Chain { get; }
}