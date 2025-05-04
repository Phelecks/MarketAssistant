using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class NftTradedEvent(Nethereum.Signer.Chain chain, string transactionHash, DateTime dateTime,
    INftTradedEvent.Erc721TransferDto erc721Transfer, List<INftTradedEvent.Erc20TransferDto> erc20TransferDto) : INftTradedEvent
{
    public Nethereum.Signer.Chain Chain { get; } = chain;
    public string TransactionHash { get; } = transactionHash;
    public DateTime DateTime { get; } = dateTime;
    public INftTradedEvent.Erc721TransferDto Erc721Transfer { get; } = erc721Transfer;
    public List<INftTradedEvent.Erc20TransferDto> Erc20Transfers { get; } = erc20TransferDto;
}