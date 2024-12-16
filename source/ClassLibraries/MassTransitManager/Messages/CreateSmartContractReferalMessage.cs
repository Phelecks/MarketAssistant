using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateSmartContractReferralMessage : ICreateSmartContractReferralMessage
{
    public CreateSmartContractReferralMessage(Guid correlationId, long tokenId, float referralPercent)
    {
        CorrelationId = correlationId;
        TokenId = tokenId;
        ReferralPercent = referralPercent;
    }

    public Guid CorrelationId { get; }

    public long TokenId { get; }

    public float ReferralPercent { get; }
}