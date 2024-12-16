using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IMatchAccountCreatedEvent : CorrelatedBy<Guid>
{
}