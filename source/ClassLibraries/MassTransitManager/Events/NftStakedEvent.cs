using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class NftStakedEvent(Nethereum.Signer.Chain chain, string transactionHash, DateTime dateTime, INftStakedEvent.Erc721TransferDto erc721Transfer) : INftStakedEvent
{
    public Nethereum.Signer.Chain Chain { get; } = chain;
    public string TransactionHash { get; } = transactionHash;
    public DateTime DateTime { get; } = dateTime;
    public INftStakedEvent.Erc721TransferDto Erc721Transfer { get; } = erc721Transfer;
}