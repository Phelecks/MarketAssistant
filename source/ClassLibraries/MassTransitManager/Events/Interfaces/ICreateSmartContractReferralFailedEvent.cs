using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ICreateSmartContractReferralFailedEvent : CorrelatedBy<Guid>
{
    string ErrorMessage { get; }
}