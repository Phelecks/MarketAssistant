using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class CreateCustomerFailedEvent : ICreateCustomerFailedEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public CreateCustomerFailedEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}