using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IMarkBasketAsPaidFailedEvent : CorrelatedBy<Guid>
{
    string ErrorMessage { get; }
}