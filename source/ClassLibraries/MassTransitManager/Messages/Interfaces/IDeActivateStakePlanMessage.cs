using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface IDeActivateStakePlanMessage : CorrelatedBy<Guid>
{
    string UserId { get; }
    long SmartContractExternalTokenId { get; }
    int NftId { get; }
    DateTime EndDateTime { get; }
}