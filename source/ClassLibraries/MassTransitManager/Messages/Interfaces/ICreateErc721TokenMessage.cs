namespace MassTransitManager.Messages.Interfaces;

public interface ICreateErc721TokenMessage
{
    long TokenId { get; }
    string Title { get; }
    int ChainId { get; }
    string ContractAddress { get; }
    string OwnerWalletAddress { get; }
    string StakeContractAddress { get; }
    string? RoyaltyWalletAddress { get; }
    decimal RoyaltyPercent { get; }
    bool EnableLogProcessing { get; }
    bool GenerateFinancialDocumentForLogProcessing { get; }
    public int Decimals { get; }
}