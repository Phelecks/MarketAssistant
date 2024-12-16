using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IStockRemovedEvent : CorrelatedBy<Guid>
{
}