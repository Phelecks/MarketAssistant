using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class TransferFailedEvent(int chain, string hash) : ITransferFailedEvent
{
    public int Chain { get; } = chain;
    public string Hash { get; } = hash;
}