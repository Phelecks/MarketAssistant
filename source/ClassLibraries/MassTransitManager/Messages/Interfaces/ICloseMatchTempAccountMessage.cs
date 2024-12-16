namespace MassTransitManager.Messages.Interfaces;

public interface ICloseMatchTempAccountMessage
{
    string Game { get; }
    long MatchId { get; }
}