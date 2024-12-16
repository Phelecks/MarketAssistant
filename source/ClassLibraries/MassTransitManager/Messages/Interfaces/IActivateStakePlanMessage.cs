using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface IActivateStakePlanMessage : CorrelatedBy<Guid>
{
    string UserId { get; }
    long SmartContractExternalTokenId { get; }
    int NftId { get; }
    DateTime StartDateTime { get; }
}