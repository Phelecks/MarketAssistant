using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class ParentOfCustomerForNftStakedFoundEvent : IParentOfCustomerForNftStakedFoundEvent
{
    public Guid CorrelationId { get; }

    public string ParentUserId { get; }

    public ParentOfCustomerForNftStakedFoundEvent(Guid correlationId, string parentUserId)
    {
        CorrelationId = correlationId;
        ParentUserId = parentUserId;
    }
}