namespace MassTransitManager.Events.Interfaces;

public interface IMatchWalletClosedEvent
{
    string Game { get; }
    long MatchId { get; }
}