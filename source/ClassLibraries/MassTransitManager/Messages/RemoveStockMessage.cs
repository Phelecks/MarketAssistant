using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class RemoveStockMessage : IRemoveStockMessage
{
    public RemoveStockMessage(Guid correlationId, long productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
        CorrelationId = correlationId;
    }

    public long ProductId { get; }

    public int Quantity { get; }

    public Guid CorrelationId { get; }
}