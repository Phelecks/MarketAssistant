using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CloseMatchTempAccountMessage : ICloseMatchTempAccountMessage
{
    public CloseMatchTempAccountMessage(string game, long matchId)
    {
        Game = game;
        MatchId = matchId;
    }

    public string Game { get; }
    public long MatchId { get; }
}