using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface IDeActivateSmartContractReferralMessage : CorrelatedBy<Guid>
{
    string UserId { get; }
    long SmartContractExternalTokenId { get; }
}