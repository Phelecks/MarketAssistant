using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class NftStakedEvent : INftStakedEvent
{
    public NftStakedEvent(string transactionHash, string erc721From, string erc721To, int nftId, DateTime dateTime, INftStakedEvent.SmartContractDto smartContract, INftStakedEvent.TradeDto? relatedTrade)
    {
        TransactionHash = transactionHash;
        Erc721From = erc721From;
        Erc721To = erc721To;
        NftId = nftId;
        DateTime = dateTime;
        SmartContract = smartContract;
        RelatedTrade = relatedTrade;
    }

    public string TransactionHash { get; }

    public string Erc721From { get; }

    public string Erc721To { get; }

    public int NftId { get; }

    public DateTime DateTime { get; }

    public INftStakedEvent.SmartContractDto SmartContract { get; }

    public INftStakedEvent.TradeDto? RelatedTrade { get; }
}