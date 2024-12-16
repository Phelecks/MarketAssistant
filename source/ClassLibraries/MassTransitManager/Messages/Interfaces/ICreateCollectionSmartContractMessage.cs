using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface ICreateCollectionSmartContractMessage : CorrelatedBy<Guid>
{
    string Title { get; }
    string ContractAddress { get; }
    string OwnerWalletAddress { get; }
    string OwnerWalletPrivateKey { get; }
    string StakeContractAddress { get; }
    string? RoyaltyWalletAddress { get; }
    decimal RoyaltyPercent { get; }
    int ChainId { get; }
    long TokenId { get; }
}