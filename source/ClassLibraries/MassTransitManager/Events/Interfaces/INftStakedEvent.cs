namespace MassTransitManager.Events.Interfaces;

public interface INftStakedEvent
{
    string TransactionHash { get; }
    string Erc721From { get; }
    string Erc721To { get; }
    int NftId { get; }
    DateTime DateTime { get; }
    SmartContractDto SmartContract { get; }
    TradeDto? RelatedTrade { get; }


    record SmartContractDto(string ContractAddress, string OwnerWalletAddress, string? RoyaltyWalletAddress, int ChainId, long ExternalTokenId);
    record TradeDto(string TransactionHash, string Erc721From, string Erc721To, DateTime DateTime, decimal TradeValue, decimal CompanyValue, List<Erc20TransferDto> Erc20Transfers);
    record Erc20TransferDto(string From, string To, decimal Value, long externalTokenId);
}