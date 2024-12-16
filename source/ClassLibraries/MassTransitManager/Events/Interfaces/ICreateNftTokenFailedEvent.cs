using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ICreateNftTokenFailedEvent : CorrelatedBy<Guid>
{
    string ErrorMessage { get; }
}