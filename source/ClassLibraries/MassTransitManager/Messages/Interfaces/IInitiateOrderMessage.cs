using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface IInitiateOrderMessage : CorrelatedBy<Guid>
{
    string UserId { get; }
    Guid BasketId { get; }
    DateTime DateTime { get; }
}