using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class MarkBasketAsPaidMessage : IMarkBasketAsPaidMessage
{
    public Guid CorrelationId { get; }

    public Guid BasketId { get;  }

    /// <summary>
    /// Constructor
    /// </summary>
    public MarkBasketAsPaidMessage(Guid correlationId, Guid basketId)
    {
        BasketId = basketId;
        CorrelationId = correlationId;
    }
}