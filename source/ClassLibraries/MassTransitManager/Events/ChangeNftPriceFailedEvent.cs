using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class ChangeNftPriceFailedEvent : IChangeNftPriceFailedEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public ChangeNftPriceFailedEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}