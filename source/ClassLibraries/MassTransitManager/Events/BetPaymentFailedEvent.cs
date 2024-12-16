using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class BetPaymentFailedEvent : IBetPaymentFailedEvent
{
    public BetPaymentFailedEvent(string game, long matchId, string transactionHash)
    {
        Game = game;
        MatchId = matchId;
        TransactionHash = transactionHash;
    }

    public string Game { get; }
    public long MatchId { get; }
    public string TransactionHash { get; }
}