using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface ICreateNftReferralRewardDocumentMessage : CorrelatedBy<Guid>
{
    string TransactionHash { get; }
    DateTime DateTime { get; }
    string Erc721From { get; }
    string Erc721To { get; }
    decimal ReferralRewardValue { get; }
    float ReferralRewardPercent { get; }
    string ReferralUserId { get; }
    SmartContractDto SmartContract { get; }
    NftDto Token { get; }
    RelatedTradeDto RelatedTrade { get; }

    record RelatedTradeDto(string TransactionHash, DateTime DateTime, List<Erc20TransferDto> Erc20Transfers);
    record Erc20TransferDto(string From, string To, decimal Value, long ExternalTokenId);
    record SmartContractDto(string ContractAddress, string OwnerWalletAddress, string? RoyaltyWalletAddress, int ChainId, long ExternalTokenId);
    record NftDto(int NftId);
}