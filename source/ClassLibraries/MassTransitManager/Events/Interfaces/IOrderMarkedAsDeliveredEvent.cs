using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IOrderMarkedAsDeliveredEvent : CorrelatedBy<Guid>
{
}