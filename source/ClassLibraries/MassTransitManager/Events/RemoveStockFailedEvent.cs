using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class RemoveStockFailedEvent : IRemoveStockFailedEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public RemoveStockFailedEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}