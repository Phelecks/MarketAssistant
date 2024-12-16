using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface ICreateTradeDocumentMessage : CorrelatedBy<Guid>
{
    string TransactionHash { get; }
    DateTime DateTime { get; }
    string Erc721From { get; }
    string Erc721To { get; }
    decimal TradeValue { get; }
    SmartContractDto SmartContract { get; }
    List<NftDto> Tokens { get; }
    List<Erc20TransferDto> Erc20Transfers { get; }


    record Erc20TransferDto(string From, string To, decimal Value, long ExternalTokenId);
    record SmartContractDto(string ContractAddress, string OwnerWalletAddress, string? RoyaltyWalletAddress, int ChainId, long ExternalTokenId);
    record NftDto(int NftId);
}