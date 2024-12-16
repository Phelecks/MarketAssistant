using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IMarkOrderAsRefundedFailedEvent : CorrelatedBy<Guid>
{
    string ErrorMessage { get; }
}