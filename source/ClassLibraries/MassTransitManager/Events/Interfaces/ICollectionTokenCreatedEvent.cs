using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ICollectionTokenCreatedEvent : CorrelatedBy<Guid>
{
    long TokenId { get; }
}