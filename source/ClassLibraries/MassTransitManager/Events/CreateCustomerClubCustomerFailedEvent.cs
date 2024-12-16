using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class CreateCustomerClubCustomerFailedEvent : ICreateCustomerClubCustomerFailedEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public CreateCustomerClubCustomerFailedEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}