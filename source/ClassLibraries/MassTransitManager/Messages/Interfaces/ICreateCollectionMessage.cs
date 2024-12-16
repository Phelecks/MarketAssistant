namespace MassTransitManager.Messages.Interfaces;

public interface ICreateCollectionMessage
{
    string Title { get; }

    int ChainId { get; }

    string ContractAddress { get; }

    string StakeContractAddress { get; }

    string OwnerWalletAddress { get; }

    string OwnerWalletPrivateKey { get; }
    string? RoyaltyWalletAddress { get; }
    decimal RoyaltyPercent { get; }
}