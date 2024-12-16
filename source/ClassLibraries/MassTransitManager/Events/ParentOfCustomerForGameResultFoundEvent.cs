using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class ParentOfCustomerForGameResultFoundEvent : IParentOfCustomerForGameResultFoundEvent
{
    public Guid CorrelationId { get; }

    public string ParentUserId { get; }

    public ParentOfCustomerForGameResultFoundEvent(Guid correlationId, string parentUserId)
    {
        CorrelationId = correlationId;
        ParentUserId = parentUserId;
    }
}