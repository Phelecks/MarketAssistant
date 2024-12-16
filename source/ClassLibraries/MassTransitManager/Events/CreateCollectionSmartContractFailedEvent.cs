using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class CreateCollectionSmartContractFailedEvent : ICreateCollectionSmartContractFailedEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public CreateCollectionSmartContractFailedEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}