using BaseDomain.Enums;
using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateCollectionSmartContractMessage : ICreateCollectionSmartContractMessage
{
    public CreateCollectionSmartContractMessage(Guid correlationId, string title, string contractAddress, string ownerWalletAddress, string ownerWalletPrivateKey, string stakeContractAddress, string? royaltyWalletAddress, int chainId, long tokenId, decimal royaltyPercent)
    {
        CorrelationId = correlationId;
        Title = title;
        ContractAddress = contractAddress;
        OwnerWalletAddress = ownerWalletAddress;
        OwnerWalletPrivateKey = ownerWalletPrivateKey;
        StakeContractAddress = stakeContractAddress;
        ChainId = chainId;
        TokenId = tokenId;
        RoyaltyWalletAddress = royaltyWalletAddress;
        RoyaltyPercent = royaltyPercent;
    }

    public Guid CorrelationId { get; }
    public string Title { get; }
    public string ContractAddress { get; }
    public string OwnerWalletAddress { get; }
    public string OwnerWalletPrivateKey { get; }
    public string StakeContractAddress { get; }
    public string? RoyaltyWalletAddress { get; }
    public int ChainId { get; }
    public decimal RoyaltyPercent { get; }
    public long TokenId { get; }
}