using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IMarkOrderAsDeliveredFailedEvent : CorrelatedBy<Guid>
{
    string ErrorMessage { get; }
}