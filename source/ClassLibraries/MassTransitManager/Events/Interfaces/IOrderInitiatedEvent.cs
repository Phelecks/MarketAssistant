using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IOrderInitiatedEvent : CorrelatedBy<Guid>
{
}