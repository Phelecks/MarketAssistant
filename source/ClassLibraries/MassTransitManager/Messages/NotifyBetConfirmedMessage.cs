using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class NotifyBetConfirmedMessage : INotifyBetConfirmedMessage
{
    public NotifyBetConfirmedMessage(string game, long matchId, Guid id, decimal value, long externalTokenId, string transactionHash, DateTime dateTime, INotifyBetConfirmedMessage.OptionDto option, string userId, INotifyBetConfirmedMessage.DiscordMessageDto? discordMessage)
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
        DiscordMessage = discordMessage;
    }

    public string Game { get; }
    public long MatchId { get; }
    public Guid Id { get; }
    public decimal Value { get; }
    public long ExternalTokenId { get; }
    public string TransactionHash { get; }
    public DateTime DateTime { get; }
    public INotifyBetConfirmedMessage.OptionDto Option { get; }
    public string UserId { get; }
    public INotifyBetConfirmedMessage.DiscordMessageDto? DiscordMessage { get; }
}
