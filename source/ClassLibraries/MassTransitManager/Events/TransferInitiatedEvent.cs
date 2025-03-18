using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class TransferInitiatedEvent(Guid correlationId, TransferInitiatedEvent.Transfer transfer) : ITransferInitiatedEvent
{
    public Guid CorrelationId { get;} = correlationId;
    public Nethereum.Signer.Chain Chain { get; } = transfer.Chain;
    public string Hash { get; } = transfer.Hash;
    public string From { get; } = transfer.From;
    public string To { get; } = transfer.To;
    public decimal Value { get; } = transfer.Value;
    public DateTime DateTime { get; } = transfer.DateTime;
    public List<ITransferInitiatedEvent.Erc20Transfer>? Erc20Transfers { get; } = transfer.Erc20Transfers;
    public List<ITransferInitiatedEvent.Erc721Transfer>? Erc721Transfers { get; } = transfer.Erc721Transfers;

    public record Transfer(Nethereum.Signer.Chain Chain, string Hash, string From, string To, decimal Value, DateTime DateTime, List<ITransferInitiatedEvent.Erc20Transfer>? Erc20Transfers, List<ITransferInitiatedEvent.Erc721Transfer>? Erc721Transfers);
}