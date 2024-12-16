using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IParentOfCustomerForNftStakedNotFoundEvent : CorrelatedBy<Guid>
{
    string ErrorMessage { get; }
}