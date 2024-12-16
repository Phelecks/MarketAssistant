using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class NotifyRewardPaidMessage : INotifyRewardPaidMessage
{
    public NotifyRewardPaidMessage(string game, long matchId, Guid id, decimal value, long externalTokenId, string transactionHash, DateTime dateTime, string userId, INotifyRewardPaidMessage.RewardType type, INotifyRewardPaidMessage.DiscordMessageDto? discordMessage)
    {
        Game = game;
        MatchId = matchId;
        Id = id;
        Value = value;
        ExternalTokenId = externalTokenId;
        TransactionHash = transactionHash;
        DateTime = dateTime;
        UserId = userId;
        Type = type;
        DiscordMessage = discordMessage;
    }

    public string Game { get; }
    public long MatchId { get; }
    public Guid Id { get; }
    public decimal Value { get; }
    public long ExternalTokenId { get; }
    public string TransactionHash { get; }
    public DateTime DateTime { get; }
    public string UserId { get; }
    public INotifyRewardPaidMessage.RewardType Type { get; }
    public INotifyRewardPaidMessage.DiscordMessageDto? DiscordMessage { get; }
}
