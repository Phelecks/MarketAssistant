using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface IAddStockMessage : CorrelatedBy<Guid>
{
    long ProductId { get; }
    public int Quantity { get; }
}