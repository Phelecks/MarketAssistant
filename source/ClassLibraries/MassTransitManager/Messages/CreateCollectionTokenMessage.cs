using BaseDomain.Enums;
using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateCollectionTokenMessage : ICreateCollecionTokenMessage
{
    public CreateCollectionTokenMessage(Guid correlationId, string symbol, string contractAddress, string ownerWalletAddress, string ownerWalletPrivateKey, string? royaltyWalletAddress, string stakeContractAddress, int chainId, decimal royaltyPercent)
    {
        CorrelationId = correlationId;
        Symbol = symbol;
        ContractAddress = contractAddress;
        OwnerWalletAddress = ownerWalletAddress;
        OwnerWalletPrivateKey = ownerWalletPrivateKey;
        StakeContractAddress = stakeContractAddress;
        ChainId = chainId;
        RoyaltyWalletAddress = royaltyWalletAddress;
        RoyaltyPercent = royaltyPercent;
    }

    public Guid CorrelationId { get; }
    public string Symbol { get; }
    public string ContractAddress { get; }
    public string OwnerWalletAddress { get; }
    public string OwnerWalletPrivateKey { get; }
    public string StakeContractAddress { get; }
    public string? RoyaltyWalletAddress { get; }
    public int ChainId { get; }
    public decimal RoyaltyPercent { get; }
}