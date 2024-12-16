using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class MatchWalletClosedEvent : IMatchWalletClosedEvent
{
    public MatchWalletClosedEvent(string game, long matchId)
    {
        Game = game;
        MatchId = matchId;
    }

    public string Game { get; }
    public long MatchId { get; }
}