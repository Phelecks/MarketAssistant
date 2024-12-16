using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class ActivateStakePlanMessage : IActivateStakePlanMessage
{
    public ActivateStakePlanMessage(Guid correlationId, string userId, long smartContractExternalTokenId, int nftId, DateTime startDateTime)
    {
        CorrelationId = correlationId;
        UserId = userId;
        SmartContractExternalTokenId = smartContractExternalTokenId;
        StartDateTime = startDateTime;
        NftId = nftId;
    }

    public Guid CorrelationId { get; }
    public string UserId { get; }
    public long SmartContractExternalTokenId { get; }
    public int NftId { get; }
    public DateTime StartDateTime { get; }
}