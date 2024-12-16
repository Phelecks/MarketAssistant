using BaseDomain.Enums;
using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateCollectionMessage : ICreateCollectionMessage
{
    public string Title { get; }

    public int ChainId { get; }

    public string ContractAddress { get; }

    public string StakeContractAddress { get; }

    public string OwnerWalletAddress { get; }

    public string OwnerWalletPrivateKey { get; }
    public string? RoyaltyWalletAddress { get; }

    public decimal RoyaltyPercent { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="clientId"></param>
    public CreateCollectionMessage(string title, int chainId, string contractAddress, string stakeContractAddress, string ownerWalletAddress, string ownerWalletPrivateKey, string? royaltyWalletAddress, decimal royaltyPercent)
    {
        Title = title;
        ChainId = chainId;
        ContractAddress = contractAddress;
        StakeContractAddress = stakeContractAddress;
        OwnerWalletAddress = ownerWalletAddress;
        OwnerWalletPrivateKey = ownerWalletPrivateKey;
        RoyaltyWalletAddress = royaltyWalletAddress;
        RoyaltyPercent = royaltyPercent;
    }
}