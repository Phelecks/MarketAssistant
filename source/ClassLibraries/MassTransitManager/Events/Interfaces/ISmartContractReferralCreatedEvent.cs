using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ISmartContractReferralCreatedEvent : CorrelatedBy<Guid>
{
    long SmartContractReferralId { get; }
}