using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ICreateBlockProcessingWalletAddressFailedEvent : CorrelatedBy<Guid>
{
    public string ErrorMessage { get; }
}