using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class MarkOrderAsReversedMessage : IMarkOrderAsReversedMessage
{
    public MarkOrderAsReversedMessage(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CorrelationId { get; }
}