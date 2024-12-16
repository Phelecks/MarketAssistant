using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateErc721TokenMessage : ICreateErc721TokenMessage
{
    public CreateErc721TokenMessage(long tokenId, string title, int chainId, string contractAddress, string ownerWalletAddress, string stakeContractAddress, string? royaltyWalletAddress, decimal royaltyPercent, bool enableLogProcessor, int decimals, bool generateFinancialDocumentForLogProcessing)
    {
        TokenId = tokenId;
        Title = title;
        ChainId = chainId;
        ContractAddress = contractAddress;
        OwnerWalletAddress = ownerWalletAddress;
        StakeContractAddress = stakeContractAddress;
        RoyaltyWalletAddress = royaltyWalletAddress;
        RoyaltyPercent = royaltyPercent;
        EnableLogProcessing = enableLogProcessor;
        Decimals = decimals;
        GenerateFinancialDocumentForLogProcessing = generateFinancialDocumentForLogProcessing;
    }

    public long TokenId { get; }
    public string Title { get; }
    public int ChainId { get; }
    public string ContractAddress { get; }
    public string OwnerWalletAddress { get; }
    public string StakeContractAddress { get; }
    public string? RoyaltyWalletAddress { get; }
    public decimal RoyaltyPercent { get; }
    public bool EnableLogProcessing { get; }
    public int Decimals { get; }
    public bool GenerateFinancialDocumentForLogProcessing { get; }
}