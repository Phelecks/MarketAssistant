using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateErc20TokenMessage : ICreateErc20TokenMessage
{
    public CreateErc20TokenMessage(long tokenId, string symbol, int chainId, string contractAddress, string ownerWalletAddress, string stakeContractAddress, bool enableLogProcessing, int decimals, bool generateFinancialDocumentForLogProcessing)
    {
        TokenId = tokenId;
        Symbol = symbol;
        ChainId = chainId;
        ContractAddress = contractAddress;
        OwnerWalletAddress = ownerWalletAddress;
        StakeContractAddress = stakeContractAddress;
        EnableLogProcessing = enableLogProcessing;
        Decimals = decimals;
        GenerateFinancialDocumentForLogProcessing = generateFinancialDocumentForLogProcessing;
    }

    public long TokenId { get; }
    public string Symbol { get; }
    public int ChainId { get; }
    public string ContractAddress { get; }
    public string OwnerWalletAddress { get; }
    public string StakeContractAddress { get; }
    public bool EnableLogProcessing { get; }
    public int Decimals { get; }
    public bool GenerateFinancialDocumentForLogProcessing { get; }
}