

using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class ContractTriggeredEvent : IContractTriggeredEvent
{
    public ContractTriggeredEvent(List<Dictionary<string, object>> objects)
    {
        Objects = objects;
    }

    public List<Dictionary<string, object>> Objects { get; }
}