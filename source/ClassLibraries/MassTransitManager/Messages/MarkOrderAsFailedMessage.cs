using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class MarkOrderAsFailedMessage : IMarkOrderAsFailedMessage
{
    public MarkOrderAsFailedMessage(Guid correlationId, Guid transactionFlow, string message)
    {
        CorrelationId = correlationId;
        Message = message;
        TransactionFlow = transactionFlow;
    }

    public Guid CorrelationId { get; }

    public Guid TransactionFlow { get; }

    public string Message { get; }
}