using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ICreateCustomerFailedEvent : CorrelatedBy<Guid>
{
    string ErrorMessage { get; }
}