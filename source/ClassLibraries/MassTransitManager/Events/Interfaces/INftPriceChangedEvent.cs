using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface INftPriceChangedEvent : CorrelatedBy<Guid>
{
    long ProductId { get; }
    string Title { get; }
    string? Description { get; }
    decimal Price { get; }
    decimal Discount { get; }
    Uri? Uri { get; }
}