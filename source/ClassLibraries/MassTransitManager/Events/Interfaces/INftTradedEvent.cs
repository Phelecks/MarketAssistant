namespace MassTransitManager.Events.Interfaces;

public interface INftTradedEvent
{
    string TransactionHash { get; }
    DateTime DateTime { get; }
    string Erc721From { get; }
    string Erc721To { get; }
    decimal TradeValue { get; }
    SmartContractDto SmartContract { get; }
    List<NftDto> Tokens { get; }
    List<Erc20TransferDto> Transfers { get; }


    record SmartContractDto(string ContractAddress, string OwnerWalletAddress, string? RoyaltyWalletAddress, int ChainId, long ExternalTokenId);
    record NftDto(int NftId);
    record Erc20TransferDto(string From, string To, decimal Value, long externalTokenId);
}