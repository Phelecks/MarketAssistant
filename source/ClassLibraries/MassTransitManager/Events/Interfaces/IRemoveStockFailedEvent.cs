using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IRemoveStockFailedEvent : CorrelatedBy<Guid>
{
    string ErrorMessage { get; }
}