using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class NotifyBetFailedMessage : INotifyBetFailedMessage
{
    public NotifyBetFailedMessage(string game, long matchId, Guid id, decimal value, long externalTokenId, string transactionHash, DateTime dateTime, INotifyBetFailedMessage.OptionDto option, string message, string userId)
    {
        Game = game;
        MatchId = matchId;
        Id = id;
        Value = value;
        ExternalTokenId = externalTokenId;
        TransactionHash = transactionHash;
        DateTime = dateTime;
        Option = option;
        Message = message;
        UserId = userId;
    }

    public string Game { get; }
    public long MatchId { get; }
    public Guid Id { get; }
    public decimal Value { get; }
    public long ExternalTokenId { get; }
    public string TransactionHash { get; }
    public DateTime DateTime { get; }
    public INotifyBetFailedMessage.OptionDto Option { get; }
    public string Message { get; }
    public string UserId { get; }
}
