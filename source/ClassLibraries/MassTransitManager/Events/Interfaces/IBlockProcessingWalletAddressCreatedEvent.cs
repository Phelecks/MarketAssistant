using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IBlockProcessingWalletAddressCreatedEvent : CorrelatedBy<Guid>
{
    string WalletAddress { get; }
}