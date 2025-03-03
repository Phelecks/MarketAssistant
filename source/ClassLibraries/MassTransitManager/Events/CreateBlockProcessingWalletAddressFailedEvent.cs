using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class CreateBlockProcessingWalletAddressFailedEvent(Guid correlationId, string errorMessage) : ICreateBlockProcessingWalletAddressFailedEvent
{
    public Guid CorrelationId { get; } = correlationId;

    public string ErrorMessage { get; } = errorMessage;
}