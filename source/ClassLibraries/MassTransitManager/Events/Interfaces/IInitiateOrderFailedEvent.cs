using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IInitiateOrderFailedEvent : CorrelatedBy<Guid>
{
    string ErrorMessage { get; }
}