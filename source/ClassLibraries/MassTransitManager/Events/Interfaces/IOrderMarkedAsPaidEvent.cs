using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IOrderMarkedAsPaidEvent : CorrelatedBy<Guid>
{
}