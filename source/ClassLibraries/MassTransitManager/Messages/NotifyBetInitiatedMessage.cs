using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class NotifyBetInitiatedMessage(NotifyBetInitiatedMessage.MatchDto match, 
    NotifyBetInitiatedMessage.TransactionDto transaction, INotifyBetInitiatedMessage.OptionDto betOn, string userId) : INotifyBetInitiatedMessage
{
    public string Game { get; } = match.Game;
    public long MatchId { get; } = match.MatchId;
    public Guid Id { get; } = transaction.Id;
    public decimal Value { get; } = transaction.Value;
    public long ExternalTokenId { get; } = transaction.ExternalTokenId;
    public string TransactionHash { get; } = transaction.TransactionHash;
    public DateTime DateTime { get; } = transaction.DateTime;
    public INotifyBetInitiatedMessage.OptionDto Option { get; } = betOn;
    public string UserId { get; } = userId;

    public record MatchDto(string Game, long MatchId);
    public record TransactionDto(Guid Id, decimal Value, long ExternalTokenId, string TransactionHash, DateTime DateTime);
}
