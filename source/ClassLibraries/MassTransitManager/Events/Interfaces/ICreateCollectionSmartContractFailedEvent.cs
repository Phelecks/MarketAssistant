using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ICreateCollectionSmartContractFailedEvent : CorrelatedBy<Guid>
{
    string ErrorMessage { get; }
}