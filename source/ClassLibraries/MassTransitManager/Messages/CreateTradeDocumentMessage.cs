using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateTradeDocumentMessage : ICreateTradeDocumentMessage
{
    public CreateTradeDocumentMessage(Guid correlationId, string transactionHash, DateTime dateTime, string erc721From, string erc721To, decimal tradeValue, ICreateTradeDocumentMessage.SmartContractDto smartContract, List<ICreateTradeDocumentMessage.NftDto> tokens, List<ICreateTradeDocumentMessage.Erc20TransferDto> erc20Transfers)
    {
        CorrelationId = correlationId;
        TransactionHash = transactionHash;
        DateTime = dateTime;
        Erc721From = erc721From;
        Erc721To = erc721To;
        TradeValue = tradeValue;
        SmartContract = smartContract;
        Tokens = tokens;
        Erc20Transfers = erc20Transfers;
    }

    public Guid CorrelationId { get; }

    public string TransactionHash { get; }

    public DateTime DateTime { get; }

    public string Erc721From { get; }

    public string Erc721To { get; }

    public decimal TradeValue { get; }

    public ICreateTradeDocumentMessage.SmartContractDto SmartContract { get; }

    public List<ICreateTradeDocumentMessage.NftDto> Tokens { get; }

    public List<ICreateTradeDocumentMessage.Erc20TransferDto> Erc20Transfers { get; }
}