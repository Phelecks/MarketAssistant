using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IMatchInitiatedEvent : CorrelatedBy<Guid>
{
    string Game { get; }
    long MatchId { get; }
    string WalletAddress { get; }
}