using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface ICreateCollecionTokenMessage : CorrelatedBy<Guid>
{
    string Symbol { get; }
    string ContractAddress { get; }
    string OwnerWalletAddress { get; }
    string OwnerWalletPrivateKey { get; }
    string StakeContractAddress { get; }
    string? RoyaltyWalletAddress { get; }
    decimal RoyaltyPercent { get; }
    int ChainId { get; }
}