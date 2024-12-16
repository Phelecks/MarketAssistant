using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface ICalculateSmartContractReferralRewardMessage : CorrelatedBy<Guid>
{
    string ParentUserId { get; }
    long SmartContractExternalTokenId { get; }
    decimal Value { get; }
}