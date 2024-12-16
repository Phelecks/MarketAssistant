using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class StableTokenCreatedEvent : IStableTokenCreatedEvent
{
    public StableTokenCreatedEvent(long tokenId, string symbol, BaseDomain.Enums.BlockChainEnums.Chain chain, string contractAddress)
    {
        TokenId = tokenId;
        Symbol = symbol;
        Chain = chain;
        ContractAddress = contractAddress;
    }

    public long TokenId { get; }
    public string Symbol { get; }
    public BaseDomain.Enums.BlockChainEnums.Chain Chain { get; }
    public string ContractAddress { get; }
}