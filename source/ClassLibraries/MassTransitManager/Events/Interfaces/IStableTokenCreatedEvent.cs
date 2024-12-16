namespace MassTransitManager.Events.Interfaces;

public interface IStableTokenCreatedEvent
{
    long TokenId { get; }
    string Symbol { get; }
    BaseDomain.Enums.BlockChainEnums.Chain Chain { get; }
    string ContractAddress { get; }
}