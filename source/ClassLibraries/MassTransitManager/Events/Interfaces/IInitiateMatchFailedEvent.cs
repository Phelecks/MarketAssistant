using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IInitiateMatchFailedEvent : CorrelatedBy<Guid>
{
    string Game { get; }
    long MatchId { get; }
    public string ErrorMessage { get; }
}