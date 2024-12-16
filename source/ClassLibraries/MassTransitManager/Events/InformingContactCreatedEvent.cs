using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class InformingContactCreatedEvent : IInformingContactCreatedEvent
{
    public Guid CorrelationId { get; }

    public long ContactId { get; }

    public InformingContactCreatedEvent(Guid correlationId, long contactId)
    {
        CorrelationId = correlationId;
        ContactId = contactId;
    }
}