using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IParentOfCustomerForNftStakedFoundEvent : CorrelatedBy<Guid>
{
    string ParentUserId { get; }
}