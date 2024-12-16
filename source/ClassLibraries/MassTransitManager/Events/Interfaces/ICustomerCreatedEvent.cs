using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ICustomerCreatedEvent : CorrelatedBy<Guid>
{

    long CustomerId { get; }

    long GroupId { get; }
}