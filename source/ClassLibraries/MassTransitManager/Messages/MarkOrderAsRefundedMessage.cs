using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class MarkOrderAsRefundedMessage : IMarkOrderAsRefundedMessage
{
    public MarkOrderAsRefundedMessage(Guid correlationId, Guid transactionFlow)
    {
        CorrelationId = correlationId;
        TransactionFlow = transactionFlow;
    }

    public Guid CorrelationId { get; }

    public Guid TransactionFlow { get; }
}