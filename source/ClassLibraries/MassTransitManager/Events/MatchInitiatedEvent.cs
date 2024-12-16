using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class MatchInitiatedEvent : IMatchInitiatedEvent
{
    public Guid CorrelationId { get; }
    public string Game { get; }
    public long MatchId { get; }
    public string WalletAddress { get; }

    public MatchInitiatedEvent(Guid correlationId, string game, long matchId, string walletAddress)
    {
        CorrelationId = correlationId;
        Game = game;
        MatchId = matchId;
        WalletAddress = walletAddress;
    }
}