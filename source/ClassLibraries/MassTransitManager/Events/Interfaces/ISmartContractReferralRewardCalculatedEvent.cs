using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ISmartContractReferralRewardCalculatedEvent : CorrelatedBy<Guid>
{
    decimal Reward { get; }
    float Percent { get; }
}