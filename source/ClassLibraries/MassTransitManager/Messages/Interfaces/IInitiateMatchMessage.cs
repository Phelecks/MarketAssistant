namespace MassTransitManager.Messages.Interfaces;

public interface IInitiateMatchMessage
{
    string Game { get; }
    long MatchId { get; }
}