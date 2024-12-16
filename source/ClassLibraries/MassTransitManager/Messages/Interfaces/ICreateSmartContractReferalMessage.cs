using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface ICreateSmartContractReferralMessage : CorrelatedBy<Guid>
{
    long TokenId { get; }
    public float ReferralPercent { get; }
}