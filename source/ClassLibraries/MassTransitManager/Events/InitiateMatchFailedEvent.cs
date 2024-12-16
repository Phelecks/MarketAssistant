using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class InitiateMatchFailedEvent : IInitiateMatchFailedEvent
{
    public Guid CorrelationId { get; }
    public string Game { get; }
    public long MatchId { get; }
    public string ErrorMessage { get; }

    public InitiateMatchFailedEvent(Guid correlationId, string game, long matchId, string errorMessage)
    {
        CorrelationId = correlationId;
        Game = game;
        MatchId = matchId;
        ErrorMessage = errorMessage;
    }
}