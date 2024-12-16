using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ICollectionSmartContractCreatedEvent : CorrelatedBy<Guid>
{
    long SmartContractId { get; }
}