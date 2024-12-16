namespace MassTransitManager.Messages.Interfaces;

public interface IUpdateErc721TokenMessage
{
    long TokenId { get; }
    string Symbol { get; }
    int ChainId { get; }
    string ContractAddress { get; }
    string StakeContractAddress { get; }
    string OwnerWalletAddress { get; }
    string? RoyaltyWalletAddress { get; }
    decimal RoyaltyPercent { get; }
    int Decimals { get; }
}