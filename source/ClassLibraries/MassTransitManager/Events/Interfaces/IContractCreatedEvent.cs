using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IContractCreatedEvent
{
    string Title { get; }
}