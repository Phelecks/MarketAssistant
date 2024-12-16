namespace MassTransitManager.Messages.Interfaces;

public interface ICloseMatchWalletMessage
{
    string Game { get; }
    long MatchId { get; }
    string MatchWalletAddress { get; }
    string DestinationAddress { get; }
}