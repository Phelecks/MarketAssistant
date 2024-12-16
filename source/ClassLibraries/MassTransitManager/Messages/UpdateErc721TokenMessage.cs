using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class UpdateErc721TokenMessage : IUpdateErc721TokenMessage
{
    public UpdateErc721TokenMessage(long tokenId, string symbol, int chainId,
        string contractAddress, string stakeContractAddress, string ownerWalletAddress, string? royaltyWalletAddress, decimal royaltyPercent, int decimals)
    {
        TokenId = tokenId;
        Symbol = symbol;
        ChainId = chainId;
        ContractAddress = contractAddress;
        StakeContractAddress = stakeContractAddress;
        OwnerWalletAddress = ownerWalletAddress;
        RoyaltyWalletAddress = royaltyWalletAddress;
        RoyaltyPercent = royaltyPercent;
        Decimals = decimals;
    }

    public long TokenId { get; }
    public string Symbol { get; }
    public int ChainId { get; }
    public string ContractAddress { get; }
    public string StakeContractAddress { get; }
    public string OwnerWalletAddress { get; }
    public string? RoyaltyWalletAddress { get; }
    public decimal RoyaltyPercent { get; }
    public int Decimals { get; }
}