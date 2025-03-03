using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class CreateBlockProcessingWalletAddressEvent(Guid correlationId, string walletAddress) : ICreateBlockProcessingWalletAddressEvent
{
    public Guid CorrelationId { get; } = correlationId;
    public string WalletAddress { get; } = walletAddress;
}