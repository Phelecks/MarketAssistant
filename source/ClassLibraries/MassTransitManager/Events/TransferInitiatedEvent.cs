using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class TransferInitiatedEvent(int chain, string hash, string from, string to, decimal value, DateTime dateTime, TransferInitiatedEvent.TransferDetails transferDetails) : ITransferInitiatedEvent
{
    public int Chain { get; } = chain;
    public string Hash { get; } = hash;
    public string From { get; } = from;
    public string To { get; } = to;
    public decimal Value { get; } = value;
    public DateTime DateTime { get; } = dateTime;
    public List<ITransferInitiatedEvent.Erc20Transfer>? Erc20Transfers { get; } = transferDetails.Erc20Transfers;
    public List<ITransferInitiatedEvent.Erc721Transfer>? Erc721Transfers { get; } = transferDetails.Erc721Transfers;

    public class TransferDetails(List<ITransferInitiatedEvent.Erc20Transfer>? erc20Transfers, List<ITransferInitiatedEvent.Erc721Transfer>? erc721Transfers)
    {
        public List<ITransferInitiatedEvent.Erc20Transfer>? Erc20Transfers { get; } = erc20Transfers;
        public List<ITransferInitiatedEvent.Erc721Transfer>? Erc721Transfers { get; } = erc721Transfers;
    }
}