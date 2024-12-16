using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface IMarkOrderAsDeliveredMessage : CorrelatedBy<Guid>
{
    Guid TransactionFlow { get; }
}