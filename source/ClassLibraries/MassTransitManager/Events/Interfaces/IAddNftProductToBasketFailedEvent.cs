using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IAddNftToBasketFailedEvent : CorrelatedBy<Guid>
{
    public string ErrorMessage { get; }
}