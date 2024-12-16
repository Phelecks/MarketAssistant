using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class ParentOfCustomerForNftStakedNotFoundEvent : IParentOfCustomerForNftStakedNotFoundEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public ParentOfCustomerForNftStakedNotFoundEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}