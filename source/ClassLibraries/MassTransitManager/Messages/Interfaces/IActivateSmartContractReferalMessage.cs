using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface IActivateSmartContractReferralMessage : CorrelatedBy<Guid>
{
    string UserId { get; }
    long SmartContractExternalTokenId { get; }
}