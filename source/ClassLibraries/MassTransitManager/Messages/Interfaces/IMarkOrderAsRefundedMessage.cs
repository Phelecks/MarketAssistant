using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface IMarkOrderAsRefundedMessage : CorrelatedBy<Guid>
{
    Guid TransactionFlow { get; }
}