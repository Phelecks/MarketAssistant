using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class CalculateSmartContractReferralRewardFailedEvent : ICalculateSmartContractReferralRewardFailedEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public CalculateSmartContractReferralRewardFailedEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}