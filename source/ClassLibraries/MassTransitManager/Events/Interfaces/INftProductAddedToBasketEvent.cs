using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface INftAddedToBasketEvent : CorrelatedBy<Guid>
{
    Guid BasketId { get; }
}