namespace MassTransitManager.Events.Interfaces;

public interface IContractTriggeredEvent
{
    List<Dictionary<string, object>> Objects { get; }
}