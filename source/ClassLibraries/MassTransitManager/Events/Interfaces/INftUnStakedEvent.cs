namespace MassTransitManager.Events.Interfaces;

public interface INftUnStakedEvent
{
    string TransactionHash { get; }
    string Erc721From { get; }
    string Erc721To { get; }
    int NftId { get; }
    DateTime DateTime { get; }
    SmartContractDto SmartContract { get; }


    record SmartContractDto(string ContractAddress, string OwnerWalletAddress, string? RoyaltyWalletAddress, int ChainId, long ExternalTokenId);
}