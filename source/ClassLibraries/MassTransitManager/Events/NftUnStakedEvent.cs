using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class NftUnStakedEvent(Nethereum.Signer.Chain chain, string transactionHash, DateTime dateTime, INftUnStakedEvent.Erc721TransferDto erc721Transfer) : INftUnStakedEvent
{
    public Nethereum.Signer.Chain Chain { get; } = chain;
    public string TransactionHash { get; } = transactionHash;
    public DateTime DateTime { get; } = dateTime;
    public INftUnStakedEvent.Erc721TransferDto Erc721Transfer { get; } = erc721Transfer;
}