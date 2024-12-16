using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class CollectionTokenCreatedEvent : ICollectionTokenCreatedEvent
{
    public Guid CorrelationId { get; }

    public long TokenId { get; }

    public CollectionTokenCreatedEvent(Guid correlationId, long tokenid)
    {
        CorrelationId = correlationId;
        TokenId = tokenid;
    }
}