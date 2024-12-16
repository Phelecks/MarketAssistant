using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class NftTradedEvent : INftTradedEvent
{
    public NftTradedEvent(string transactionHash, DateTime dateTime, string erc721From, string erc721To, decimal tradeValue, INftTradedEvent.SmartContractDto smartContract, List<INftTradedEvent.NftDto> tokens, List<INftTradedEvent.Erc20TransferDto> transfers)
    {
        TransactionHash = transactionHash;
        DateTime = dateTime;
        Erc721From = erc721From;
        Erc721To = erc721To;
        TradeValue = tradeValue;
        SmartContract = smartContract;
        Tokens = tokens;
        Transfers = transfers;
    }

    public string TransactionHash { get; }

    public DateTime DateTime { get; }

    public string Erc721From { get; }

    public string Erc721To { get; }

    public decimal TradeValue { get; }

    public INftTradedEvent.SmartContractDto SmartContract { get; }

    public List<INftTradedEvent.NftDto> Tokens { get; }

    public List<INftTradedEvent.Erc20TransferDto> Transfers { get; }
}