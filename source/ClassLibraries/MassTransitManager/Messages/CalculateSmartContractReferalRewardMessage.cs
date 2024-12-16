using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CalculateSmartContractReferralRewardMessage : ICalculateSmartContractReferralRewardMessage
{
    public CalculateSmartContractReferralRewardMessage(Guid correlationId, string parentUserId, long smartContractExternalTokenId, decimal value)
    {
        CorrelationId = correlationId;
        ParentUserId = parentUserId;
        SmartContractExternalTokenId = smartContractExternalTokenId;
        Value = value;
    }

    public Guid CorrelationId { get; }
    public string ParentUserId { get; }
    public long SmartContractExternalTokenId { get; }
    public decimal Value { get; }
}