using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateMatchWalletMessage : ICreateMatchWalletMessage
{
    public CreateMatchWalletMessage(Guid correlationId, string game, long matchId)
    {
        CorrelationId = correlationId;
        Game = game;
        MatchId = matchId;
    }

    public Guid CorrelationId { get; }
    public string Game { get; }
    public long MatchId { get; }
}