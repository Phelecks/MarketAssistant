using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IOrderMarkedAsRefundedEvent : CorrelatedBy<Guid>
{
}