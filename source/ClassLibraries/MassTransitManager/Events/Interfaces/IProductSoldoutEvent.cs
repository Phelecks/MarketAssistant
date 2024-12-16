namespace MassTransitManager.Events.Interfaces;

public interface IProductSoldoutEvent
{
    long ProductId { get; }
}