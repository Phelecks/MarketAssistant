using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ITradeDocumentCreatedEvent : CorrelatedBy<Guid>
{
    public Guid DocumentId { get; }
}