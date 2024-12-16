using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IMarkOrderAsPaidFailedEvent : CorrelatedBy<Guid>
{
    string ErrorMessage { get; }
}