using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class InitiateOrderMessage : IInitiateOrderMessage
{
    public InitiateOrderMessage(Guid correlationId, string userId, Guid basketId, DateTime dateTime)
    {
        CorrelationId = correlationId;
        UserId = userId;
        BasketId = basketId;
        DateTime = dateTime;
    }

    public Guid CorrelationId { get; }

    public string UserId { get; }

    public Guid BasketId { get; }
    public DateTime DateTime { get; }
}