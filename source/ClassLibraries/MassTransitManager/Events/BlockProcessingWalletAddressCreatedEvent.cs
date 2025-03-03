using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class BlockProcessingWalletAddressCreatedEvent(Guid correlationId, string walletAddress) : IBlockProcessingWalletAddressCreatedEvent
{
    public Guid CorrelationId { get; } = correlationId;

    public string WalletAddress { get; } = walletAddress;
}