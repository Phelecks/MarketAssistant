using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ICreateMatchAccountFailedEvent : CorrelatedBy<Guid>
{
    public string ErrorMessage { get; }
}