using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CloseMatchWalletMessage : ICloseMatchWalletMessage
{
    public CloseMatchWalletMessage(string game, long matchId, string matchWalletAddress, string destinationAddress)
    {
        Game = game;
        MatchId = matchId;
        MatchWalletAddress = matchWalletAddress;
        DestinationAddress = destinationAddress;
    }

    public string Game { get; }

    public long MatchId { get; }

    public string MatchWalletAddress { get; }
    public string DestinationAddress { get; }
}