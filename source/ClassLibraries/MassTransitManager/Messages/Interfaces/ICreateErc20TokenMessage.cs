namespace MassTransitManager.Messages.Interfaces;

public interface ICreateErc20TokenMessage
{
    long TokenId { get; }
    string Symbol { get; }
    int ChainId { get; }
    string ContractAddress { get; }
    string OwnerWalletAddress { get; }
    string StakeContractAddress { get; }
    bool EnableLogProcessing { get; }
    bool GenerateFinancialDocumentForLogProcessing { get; }
    public int Decimals { get; }
}