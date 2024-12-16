using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class MatchAccountCreatedEvent : IMatchAccountCreatedEvent
{
    public Guid CorrelationId { get; }

    public MatchAccountCreatedEvent(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
}