using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class CreateNftTokenFailedEvent : ICreateNftTokenFailedEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public CreateNftTokenFailedEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}