using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class NftUnStakedEvent : INftUnStakedEvent
{
    public NftUnStakedEvent(string transactionHash, string erc721From, string erc721To, int nftId, DateTime dateTime, INftUnStakedEvent.SmartContractDto smartContract)
    {
        TransactionHash = transactionHash;
        Erc721From = erc721From;
        Erc721To = erc721To;
        NftId = nftId;
        DateTime = dateTime;
        SmartContract = smartContract;
    }

    public string TransactionHash { get; }

    public string Erc721From{ get; }

    public string Erc721To{ get; }

    public int NftId{ get; }

    public DateTime DateTime{ get; }

    public INftUnStakedEvent.SmartContractDto SmartContract{ get; }
}