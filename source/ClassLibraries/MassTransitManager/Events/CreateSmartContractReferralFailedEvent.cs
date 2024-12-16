using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class CreateSmartContractReferralFailedEvent : ICreateSmartContractReferralFailedEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public CreateSmartContractReferralFailedEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}