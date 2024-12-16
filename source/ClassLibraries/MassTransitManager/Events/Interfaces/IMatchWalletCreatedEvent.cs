using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IMatchWalletCreatedEvent : CorrelatedBy<Guid>
{
    string WalletAddress { get; }
}