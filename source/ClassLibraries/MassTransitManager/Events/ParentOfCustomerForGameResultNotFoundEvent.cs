using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class ParentOfCustomerForGameResultNotFoundEvent : IParentOfCustomerForGameResultNotFoundEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public ParentOfCustomerForGameResultNotFoundEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}