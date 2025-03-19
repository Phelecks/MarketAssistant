using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class TransferConfirmedMessage(Guid correlationId, TransferConfirmedMessage.Transfer transfer) : ITransferConfirmedMessage
{
    public Guid CorrelationId { get;} = correlationId;
    public Nethereum.Signer.Chain Chain { get; } = transfer.Chain;
    public string Hash { get; } = transfer.Hash;
    public string From { get; } = transfer.From;
    public string To { get; } = transfer.To;
    public decimal Value { get; } = transfer.Value;
    public DateTime DateTime { get; } = transfer.DateTime;
    public List<ITransferConfirmedMessage.Erc20Transfer>? Erc20Transfers { get; } = transfer.Erc20Transfers;
    public List<ITransferConfirmedMessage.Erc721Transfer>? Erc721Transfers { get; } = transfer.Erc721Transfers;

    public record Transfer(Nethereum.Signer.Chain Chain, string Hash, string From, string To, decimal Value, DateTime DateTime, List<ITransferConfirmedMessage.Erc20Transfer>? Erc20Transfers, List<ITransferConfirmedMessage.Erc721Transfer>? Erc721Transfers);
}