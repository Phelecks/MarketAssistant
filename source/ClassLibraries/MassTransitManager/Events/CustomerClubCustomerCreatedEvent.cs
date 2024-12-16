using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class CustomerClubCustomerCreatedEvent : ICustomerClubCustomerCreatedEvent
{
    public Guid CorrelationId { get; }

    public long CustomerId { get; }

    public CustomerClubCustomerCreatedEvent(Guid correlationId, long customerId)
    {
        CorrelationId = correlationId;
        CustomerId = customerId;
    }
}