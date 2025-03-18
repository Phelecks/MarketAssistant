using BaseDomain.Common;

namespace BlockChain.Domain.Events.Notification;

public class NotificationCreatedEvent(Guid correlationId, string walletAddress, NotificationCreatedEvent.Transfer transfer) : BaseEvent
{
    public Guid CorrelationId { get;} = correlationId;
    public string WalletAddress { get; } = walletAddress;
    public Nethereum.Signer.Chain Chain { get; } = transfer.Chain;
    public string Hash { get; } = transfer.Hash;
    public string From { get; } = transfer.From;
    public string To { get; } = transfer.To;
    public decimal Value { get; } = transfer.Value;
    public DateTime DateTime { get; } = transfer.DateTime;
    public List<Erc20Transfer>? Erc20Transfers { get; } = transfer.Erc20Transfers;
    public List<Erc721Transfer>? Erc721Transfers { get; } = transfer.Erc721Transfers;

    public record Transfer(Nethereum.Signer.Chain Chain, string Hash, string From, string To, decimal Value, DateTime DateTime, List<Erc20Transfer>? Erc20Transfers, List<Erc721Transfer>? Erc721Transfers);
    public record Erc20Transfer(string From, string To, decimal Value, string ContractAddress);
    public record Erc721Transfer(string From, string To, long TokenId, string ContractAddress);
}