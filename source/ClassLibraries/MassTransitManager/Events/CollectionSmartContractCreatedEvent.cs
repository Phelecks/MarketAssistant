using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class CollectionSmartContractCreatedEvent : ICollectionSmartContractCreatedEvent
{
    public CollectionSmartContractCreatedEvent(Guid correlationId, long smartContractId)
    {
        CorrelationId = correlationId;
        SmartContractId = smartContractId;
    }

    public Guid CorrelationId { get; }
    public long SmartContractId { get; }
}