using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ICreateCustomerClubCustomerFailedEvent : CorrelatedBy<Guid>
{
    string ErrorMessage { get; }
}