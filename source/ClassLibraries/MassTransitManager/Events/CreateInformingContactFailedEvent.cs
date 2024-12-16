using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class CreateInformingContactFailedEvent : ICreateInformingContactFailedEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public CreateInformingContactFailedEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}