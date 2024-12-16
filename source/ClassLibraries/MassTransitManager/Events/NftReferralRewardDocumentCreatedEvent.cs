using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class NftReferralRewardDocumentCreatedEvent : INftReferralRewardDocumentCreatedEvent
{
    public Guid CorrelationId { get; }

    public Guid DocumentId { get; }

    public NftReferralRewardDocumentCreatedEvent(Guid correlationId, Guid documentId)
    {
        CorrelationId = correlationId;
        DocumentId = documentId;
    }
}