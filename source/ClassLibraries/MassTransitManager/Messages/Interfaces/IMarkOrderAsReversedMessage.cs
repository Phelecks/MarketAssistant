using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface IMarkOrderAsReversedMessage : CorrelatedBy<Guid>
{
}