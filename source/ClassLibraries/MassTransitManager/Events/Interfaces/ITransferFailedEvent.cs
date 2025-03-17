using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ITransferFailedEvent : CorrelatedBy<Guid>
{
    string ErrorMessage { get; }
}