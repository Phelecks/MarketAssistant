using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface INftReferralRewardDocumentCreatedEvent : CorrelatedBy<Guid>
{
    public Guid DocumentId { get; }
}