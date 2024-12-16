using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ICreateNftReferralRewardDocumentFailedEvent : CorrelatedBy<Guid>
{
    string ErrorMessage { get; }
}