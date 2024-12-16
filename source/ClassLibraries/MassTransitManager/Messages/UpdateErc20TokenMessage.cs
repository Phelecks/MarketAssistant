using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class UpdateErc20TokenMessage : IUpdateErc20TokenMessage
{
    public UpdateErc20TokenMessage(long tokenId, string symbol, int chainId, string contractAddress, string ownerWalletAddress, string stakeContractAddress, bool enableLogProcessing, int decimals)
    {
        TokenId = tokenId;
        Symbol = symbol;
        ChainId = chainId;
        ContractAddress = contractAddress;
        OwnerWalletAddress = ownerWalletAddress;
        StakeContractAddress = stakeContractAddress;
        EnableLogProcessing = enableLogProcessing;
        Decimals = decimals;
    }

    public long TokenId { get; }
    public string Symbol { get; }
    public int ChainId { get; }
    public string ContractAddress { get; }
    public string OwnerWalletAddress { get; }
    public string StakeContractAddress { get; }
    public bool EnableLogProcessing { get; }
    public int Decimals { get; }
}