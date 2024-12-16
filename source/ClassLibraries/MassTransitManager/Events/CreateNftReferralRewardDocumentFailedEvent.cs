using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class CreateNftReferralRewardDocumentFailedEvent : ICreateNftReferralRewardDocumentFailedEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public CreateNftReferralRewardDocumentFailedEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}