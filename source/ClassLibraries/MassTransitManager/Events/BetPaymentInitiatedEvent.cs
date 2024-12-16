using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class BetPaymentInitiatedEvent : IBetPaymentInitiatedEvent
{
    public BetPaymentInitiatedEvent(string game, long matchId, string transactionHash, string from, string to, long externalTokenId, decimal value, DateTime dateTime)
    {
        Game = game;
        MatchId = matchId;
        TransactionHash = transactionHash;
        From = from;
        To = to;
        ExternalTokenId = externalTokenId;
        Value = value;
        DateTime = dateTime;
    }

    public string Game { get; }
    public long MatchId { get; }
    public string TransactionHash { get; }
    public string From { get; }
    public string To { get; }
    public long ExternalTokenId { get; }
    public decimal Value { get; }
    public DateTime DateTime { get; }
}