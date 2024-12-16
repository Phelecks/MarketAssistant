using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ICustomerClubCustomerCreatedEvent : CorrelatedBy<Guid>
{
    long CustomerId { get; }
}