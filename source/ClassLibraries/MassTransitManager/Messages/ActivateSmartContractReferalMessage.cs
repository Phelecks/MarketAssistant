using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class ActivateSmartContractReferralMessage : IActivateSmartContractReferralMessage
{
    public ActivateSmartContractReferralMessage(Guid correlationId, string userId, long smartContractExternalTokenId)
    {
        CorrelationId = correlationId;
        UserId = userId;
        SmartContractExternalTokenId = smartContractExternalTokenId;
    }

    public Guid CorrelationId { get; }
    public string UserId { get; }
    public long SmartContractExternalTokenId { get; }
}