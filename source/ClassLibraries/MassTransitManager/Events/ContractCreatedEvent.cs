using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class ContractCreatedEvent : IContractCreatedEvent
{
    public string Title { get; }

    public ContractCreatedEvent(string title)
    {
        Title = title;
    }
}