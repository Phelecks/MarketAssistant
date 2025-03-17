using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class TransferFailedEvent(Guid correlationId, string errorMessage) : ITransferFailedEvent
{
    public Guid CorrelationId { get;} = correlationId;
    public string ErrorMessage { get; } = errorMessage;
}