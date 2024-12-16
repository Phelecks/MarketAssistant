using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class CreateMatchWalletFailedEvent : ICreateMatchWalletFailedEvent
{
    public Guid CorrelationId { get; }

    public string ErrorMessage { get; }

    public CreateMatchWalletFailedEvent(Guid correlationId, string errorMessage)
    {
        CorrelationId = correlationId;
        ErrorMessage = errorMessage;
    }
}