using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class CreateMatchAccountFailedEvent : ICreateMatchAccountFailedEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public CreateMatchAccountFailedEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}