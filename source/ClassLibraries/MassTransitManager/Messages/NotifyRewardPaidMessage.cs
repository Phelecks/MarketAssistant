using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class NotifyRewardPaidMessage(NotifyRewardPaidMessage.MatchDto match, 
    NotifyRewardPaidMessage.TransactionDto transaction, string userId, INotifyRewardPaidMessage.RewardType type, INotifyRewardPaidMessage.DiscordMessageDto? discordMessage) : INotifyRewardPaidMessage
{
    public string Game { get; } = match.Game;
    public long MatchId { get; } = match.MatchId;
    public Guid Id { get; } = transaction.Id;
    public decimal Value { get; } = transaction.Value;
    public long ExternalTokenId { get; } = transaction.ExternalTokenId;
    public string TransactionHash { get; } = transaction.TransactionHash;
    public DateTime DateTime { get; } = transaction.DateTime;
    public string UserId { get; } = userId;
    public INotifyRewardPaidMessage.RewardType Type { get; } = type;
    public INotifyRewardPaidMessage.DiscordMessageDto? DiscordMessage { get; } = discordMessage;

    public record MatchDto(string Game, long MatchId);
    public record TransactionDto(Guid Id, decimal Value, long ExternalTokenId, string TransactionHash, DateTime DateTime);
}
