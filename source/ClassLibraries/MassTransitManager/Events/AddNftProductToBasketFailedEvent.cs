using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class AddNftToBasketFailedEvent : IAddNftToBasketFailedEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public AddNftToBasketFailedEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}