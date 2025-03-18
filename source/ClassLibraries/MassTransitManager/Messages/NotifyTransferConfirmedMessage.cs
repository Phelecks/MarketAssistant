using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class NotifyTransferConfirmedMessage(Guid correlationId, string userId, INotifyTransferConfirmedMessage.DiscordMessage? discord, NotifyTransferConfirmedMessage.Transfer transfer) : INotifyTransferConfirmedMessage
{
    public Guid CorrelationId { get;} = correlationId;
    public string UserId { get; } = userId;
    public INotifyTransferConfirmedMessage.DiscordMessage? Discord { get; } = discord;
    public Nethereum.Signer.Chain Chain { get; } = transfer.Chain;
    public string Hash { get; } = transfer.Hash;
    public string From { get; } = transfer.From;
    public string To { get; } = transfer.To;
    public decimal Value { get; } = transfer.Value;
    public DateTime DateTime { get; } = transfer.DateTime;
    public List<Events.Interfaces.ITransferConfirmedEvent.Erc20Transfer>? Erc20Transfers { get; } = transfer.Erc20Transfers;
    public List<Events.Interfaces.ITransferConfirmedEvent.Erc721Transfer>? Erc721Transfers { get; } = transfer.Erc721Transfers;

    public record Transfer(Nethereum.Signer.Chain Chain, string Hash, string From, string To, decimal Value, DateTime DateTime, List<Events.Interfaces.ITransferConfirmedEvent.Erc20Transfer>? Erc20Transfers,List<Events.Interfaces.ITransferConfirmedEvent.Erc721Transfer>? Erc721Transfers);
}
