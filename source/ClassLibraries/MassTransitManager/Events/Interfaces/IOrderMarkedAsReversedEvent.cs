using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IOrderMarkedAsReversedEvent : CorrelatedBy<Guid>
{
}