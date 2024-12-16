using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class MarkOrderAsDeliveredMessage : IMarkOrderAsDeliveredMessage
{
    public MarkOrderAsDeliveredMessage(Guid correlationId, Guid transactionFlow)
    {
        CorrelationId = correlationId;
        TransactionFlow = transactionFlow;
    }

    public Guid CorrelationId { get; }

    public Guid TransactionFlow { get; }
}