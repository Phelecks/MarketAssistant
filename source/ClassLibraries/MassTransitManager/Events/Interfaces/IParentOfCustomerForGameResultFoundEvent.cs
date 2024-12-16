using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IParentOfCustomerForGameResultFoundEvent : CorrelatedBy<Guid>
{
    string ParentUserId { get; }
}