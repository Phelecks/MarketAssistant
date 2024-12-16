using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateCloseMatchWalletDocumentMessage : ICreateCloseMatchWalletDocumentMessage
{
    public CreateCloseMatchWalletDocumentMessage(Guid transferId, string transactionHash, string from, string to, decimal value, long externalTokenId, DateTime datetime, ICreateCloseMatchWalletDocumentMessage.NetworkFeeDto networkFee)
    {
        TransferId = transferId;
        TransactionHash = transactionHash;
        From = from;
        To = to;
        ExternalTokenId = externalTokenId;
        Value = value;
        DateTime = datetime;
        NetworkFee = networkFee;
    }

    public Guid TransferId { get; }
    public string TransactionHash { get; }
    public string From { get; }
    public string To { get; }
    public long ExternalTokenId { get; }
    public decimal Value { get; }
    public DateTime DateTime { get; }
    public ICreateCloseMatchWalletDocumentMessage.NetworkFeeDto NetworkFee { get; }
}