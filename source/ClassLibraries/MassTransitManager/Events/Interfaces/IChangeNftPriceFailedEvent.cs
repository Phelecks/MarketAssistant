using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IChangeNftPriceFailedEvent : CorrelatedBy<Guid>
{
    public string ErrorMessage { get; }
}