using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class NotifyTransferConfirmedMessage(Guid correlationId, string userId, NotifyTransferConfirmedMessage.Transfer transfer, NotifyTransferConfirmedMessage.TransferDetails transferDetails) : INotifyTransferConfirmedMessage
{
    public Guid CorrelationId { get;} = correlationId;
    public string UserId { get; } = userId;
    public int Chain { get; } = transfer.Chain;
    public string Hash { get; } = transfer.Hash;
    public string From { get; } = transfer.From;
    public string To { get; } = transfer.To;
    public decimal Value { get; } = transfer.Value;
    public DateTime DateTime { get; } = transfer.DateTime;
    public List<INotifyTransferConfirmedMessage.Erc20Transfer>? Erc20Transfers { get; } = transferDetails.Erc20Transfers;
    public List<INotifyTransferConfirmedMessage.Erc721Transfer>? Erc721Transfers { get; } = transferDetails.Erc721Transfers;

    public class Transfer(int chain, string hash, string from, string to, decimal value, DateTime dateTime)
    {
        public int Chain { get; } = chain;
        public string Hash { get; } = hash;
        public string From { get; } = from;
        public string To { get; } = to;
        public decimal Value { get; } = value;
        public DateTime DateTime { get; } = dateTime;
    }
    public class TransferDetails(List<INotifyTransferConfirmedMessage.Erc20Transfer>? erc20Transfers, List<INotifyTransferConfirmedMessage.Erc721Transfer>? erc721Transfers)
    {
        public List<INotifyTransferConfirmedMessage.Erc20Transfer>? Erc20Transfers { get; } = erc20Transfers;
        public List<INotifyTransferConfirmedMessage.Erc721Transfer>? Erc721Transfers { get; } = erc721Transfers;
    }
}
