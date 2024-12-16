using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class AddStockMessage : IAddStockMessage
{
    public AddStockMessage(long productId, int quantity, Guid correlationId)
    {
        ProductId = productId;
        Quantity = quantity;
        CorrelationId = correlationId;
    }

    public long ProductId { get; }

    public int Quantity { get; }

    public Guid CorrelationId { get; }
}