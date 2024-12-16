using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class SmartContractReferralCreatedEvent : ISmartContractReferralCreatedEvent
{
    public SmartContractReferralCreatedEvent(Guid correlationId, long smartContractReferralId)
    {
        CorrelationId = correlationId;
        SmartContractReferralId = smartContractReferralId;
    }

    public Guid CorrelationId { get; }
    public long SmartContractReferralId { get; }
}