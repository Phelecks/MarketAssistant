using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class NotifyBetStatusMessage(NotifyBetStatusMessage.MatchDto match, 
    NotifyBetStatusMessage.TransactionDto transaction, INotifyBetStatusMessage.OptionDto option, string userId, INotifyBetStatusMessage.BetStatus status, INotifyBetStatusMessage.DiscordMessageDto? discordMessage) : INotifyBetStatusMessage
{
    public string Game { get; } = match.Game;
    public long MatchId { get; } = match.MatchId;
    public Guid Id { get; } = transaction.Id;
    public decimal Value { get; } = transaction.Value;
    public long ExternalTokenId { get; } = transaction.ExternalTokenId;
    public string TransactionHash { get; } = transaction.TransactionHash;
    public DateTime DateTime { get; } = transaction.DateTime;
    public INotifyBetStatusMessage.OptionDto Option { get; } = option;
    public string UserId { get; } = userId;
    public INotifyBetStatusMessage.BetStatus Status { get; } = status;
    public INotifyBetStatusMessage.DiscordMessageDto? DiscordMessage { get; } = discordMessage;

    public record MatchDto(string Game, long MatchId);
    public record TransactionDto(Guid Id, decimal Value, long ExternalTokenId, string TransactionHash, DateTime DateTime);
}
