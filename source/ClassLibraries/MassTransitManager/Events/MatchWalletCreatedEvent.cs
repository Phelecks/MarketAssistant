using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class MatchWalletCreatedEvent : IMatchWalletCreatedEvent
{
    public Guid CorrelationId { get; }

    public string WalletAddress { get; }

    public MatchWalletCreatedEvent(Guid correlationId, string walletAddress)
    {
        CorrelationId = correlationId;
        WalletAddress = walletAddress;
    }
}