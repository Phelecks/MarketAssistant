using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class DeActivateStakePlanMessage : IDeActivateStakePlanMessage
{
    public DeActivateStakePlanMessage(Guid correlationId, string userId, long smartContractExternalTokenId, int nftId, DateTime endDateTime)
    {
        CorrelationId = correlationId;
        UserId = userId;
        SmartContractExternalTokenId = smartContractExternalTokenId;
        EndDateTime = endDateTime;
        NftId = nftId;
    }

    public Guid CorrelationId { get; }
    public string UserId { get; }
    public long SmartContractExternalTokenId { get; }
    public int NftId { get; }
    public DateTime EndDateTime { get; }
}