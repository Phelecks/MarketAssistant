using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateMatchAccountMessage : ICreateMatchAccountMessage
{
    public CreateMatchAccountMessage(Guid correlationId, string game, long matchId, string walletAddress)
    {
        CorrelationId = correlationId;
        Game = game;
        MatchId = matchId;
        WalletAddress = walletAddress;
    }

    public Guid CorrelationId { get; }
    public string Game { get; }
    public long MatchId { get; }
    public string WalletAddress { get; }
}