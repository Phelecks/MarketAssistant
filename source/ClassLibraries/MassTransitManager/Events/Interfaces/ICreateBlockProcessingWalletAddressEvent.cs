using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ICreateBlockProcessingWalletAddressEvent : CorrelatedBy<Guid>
{
    string WalletAddress { get; }
}