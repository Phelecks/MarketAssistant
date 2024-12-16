using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ICalculateSmartContractReferralRewardFailedEvent : CorrelatedBy<Guid>
{
    string ErrorMessage { get; }
}