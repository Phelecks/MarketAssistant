namespace MassTransitManager.Events.Interfaces;

public interface ITokenCreatedEvent
{
    long TokenId { get; }
    string Symbol { get; }
    BaseDomain.Enums.BlockChainEnums.Chain Chain { get; }
}