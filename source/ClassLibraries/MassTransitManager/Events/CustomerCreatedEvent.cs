using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class CustomerCreatedEvent : ICustomerCreatedEvent
{
    public Guid CorrelationId { get; }

    public long CustomerId { get; }

    public long GroupId { get; }

    public CustomerCreatedEvent(Guid correlationId, long customerId, long groupId)
    {
        CorrelationId = correlationId;
        CustomerId = customerId;
        GroupId = groupId;
    }
}