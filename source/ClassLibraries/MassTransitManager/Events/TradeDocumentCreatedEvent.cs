using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class TradeDocumentCreatedEvent : ITradeDocumentCreatedEvent
{
    public Guid CorrelationId { get; }

    public Guid DocumentId { get; }

    public TradeDocumentCreatedEvent(Guid correlationId, Guid documentId)
    {
        CorrelationId = correlationId;
        DocumentId = documentId;
    }
}