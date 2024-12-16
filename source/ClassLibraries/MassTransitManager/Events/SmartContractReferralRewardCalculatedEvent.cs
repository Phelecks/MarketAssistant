using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class SmartContractReferralRewardCalculatedEvent : ISmartContractReferralRewardCalculatedEvent
{
    public Guid CorrelationId { get; }

    public decimal Reward { get; }

    public float Percent { get; }

    public SmartContractReferralRewardCalculatedEvent(Guid correlationId, decimal reward, float percent)
    {
        CorrelationId = correlationId;
        Reward = reward;
        Percent = percent;
    }
}