using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface IRemoveStockMessage : CorrelatedBy<Guid>
{
    long ProductId { get; }
    public int Quantity { get; }
}