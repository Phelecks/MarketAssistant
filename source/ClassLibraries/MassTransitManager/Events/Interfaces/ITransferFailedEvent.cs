namespace MassTransitManager.Events.Interfaces;

public interface ITransferFailedEvent
{
    int Chain { get; }
    string Hash { get; }
}