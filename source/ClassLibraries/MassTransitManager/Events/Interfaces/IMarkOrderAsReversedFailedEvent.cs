using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IMarkOrderAsReversedFailedEvent : CorrelatedBy<Guid>
{
    string ErrorMessage { get; }
}