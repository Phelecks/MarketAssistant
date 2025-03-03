using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IDeleteBlockProcessingWalletAddressEvent : CorrelatedBy<Guid>
{
    string WalletAddress { get; }
}