using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class NotifyBetInitiatedMessage : INotifyBetInitiatedMessage
{
    public NotifyBetInitiatedMessage(string game, Guid id, long matchId, decimal value, long externalTokenId, string transactionHash, DateTime dateTime, INotifyBetInitiatedMessage.OptionDto betOn, string userId)
    {
        Game = game;
        MatchId = matchId;
        Id = id;
        Value = value;
        ExternalTokenId = externalTokenId;
        TransactionHash = transactionHash;
        DateTime = dateTime;
        Option = betOn;
        UserId = userId;
    }

    public string Game { get; }
    public long MatchId { get; }
    public Guid Id { get; }
    public decimal Value { get; }
    public long ExternalTokenId { get; }
    public string TransactionHash { get; }
    public DateTime DateTime { get; }
    public INotifyBetInitiatedMessage.OptionDto Option { get; }
    public string UserId { get; }
}
