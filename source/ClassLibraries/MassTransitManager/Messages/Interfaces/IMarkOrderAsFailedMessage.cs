using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface IMarkOrderAsFailedMessage : CorrelatedBy<Guid>
{
    Guid TransactionFlow { get; }
    string? Message { get; }
}