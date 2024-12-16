using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IParentOfCustomerForGameResultNotFoundEvent : CorrelatedBy<Guid>
{
    string ErrorMessage { get; }
}