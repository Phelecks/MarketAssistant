using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class MarkOrderAsPaidMessage : IMarkOrderAsPaidMessage
{
    public MarkOrderAsPaidMessage(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CorrelationId { get; }
}