using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class CreateFinancialAccountFailedEvent : ICreateFinancialAccountFailedEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public CreateFinancialAccountFailedEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}