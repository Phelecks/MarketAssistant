using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class TransferConfirmedEvent(Guid correlationId, int chain, string hash, string from, string to, decimal value, DateTime dateTime, TransferConfirmedEvent.TransferDetails transferDetails) : ITransferConfirmedEvent
{
    public Guid CorrelationId { get;} = correlationId;
    public int Chain { get; } = chain;
    public string Hash { get; } = hash;
    public string From { get; } = from;
    public string To { get; } = to;
    public decimal Value { get; } = value;
    public DateTime DateTime { get; } = dateTime;
    public List<ITransferConfirmedEvent.Erc20Transfer>? Erc20Transfers { get; } = transferDetails.Erc20Transfers;
    public List<ITransferConfirmedEvent.Erc721Transfer>? Erc721Transfers { get; } = transferDetails.Erc721Transfers;

    public class TransferDetails(List<ITransferConfirmedEvent.Erc20Transfer>? erc20Transfers, List<ITransferConfirmedEvent.Erc721Transfer>? erc721Transfers)
    {
        public List<ITransferConfirmedEvent.Erc20Transfer>? Erc20Transfers { get; } = erc20Transfers;
        public List<ITransferConfirmedEvent.Erc721Transfer>? Erc721Transfers { get; } = erc721Transfers;
    }
}