using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class CreateTradeDocumentFailedEvent : ICreateTradeDocumentFailedEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public CreateTradeDocumentFailedEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}