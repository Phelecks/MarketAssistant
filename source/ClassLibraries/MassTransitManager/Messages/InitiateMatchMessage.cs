using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class InitiateMatchMessage : IInitiateMatchMessage
{
    public InitiateMatchMessage(string game, long matchId)
    {
        Game = game;
        MatchId = matchId;
    }

    public string Game { get; }
    public long MatchId { get; }
}