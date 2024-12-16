using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class NotifyBetStatusMessage : INotifyBetStatusMessage
{
    public NotifyBetStatusMessage(string game, long matchId, Guid id, decimal value, long externalTokenId, string transactionHash, DateTime dateTime, INotifyBetStatusMessage.OptionDto option, string userId, INotifyBetStatusMessage.BetStatus status, INotifyBetStatusMessage.DiscordMessageDto? discordMessage)
    {
        Game = game;
        MatchId = matchId;
        Id = id;
        Value = value;
        ExternalTokenId = externalTokenId;
        TransactionHash = transactionHash;
        DateTime = dateTime;
        Option = option;
        UserId = userId;
        Status = status;
        DiscordMessage = discordMessage;
    }

    public string Game { get; }
    public long MatchId { get; }
    public Guid Id { get; }
    public decimal Value { get; }
    public long ExternalTokenId { get; }
    public string TransactionHash { get; }
    public DateTime DateTime { get; }
    public INotifyBetStatusMessage.OptionDto Option { get; }
    public string UserId { get; }
    public INotifyBetStatusMessage.BetStatus Status { get; }
    public INotifyBetStatusMessage.DiscordMessageDto? DiscordMessage { get; }
}
