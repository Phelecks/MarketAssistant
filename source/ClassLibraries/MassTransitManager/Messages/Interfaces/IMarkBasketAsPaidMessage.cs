using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface IMarkBasketAsPaidMessage : CorrelatedBy<Guid>
{
    Guid BasketId { get; }
}