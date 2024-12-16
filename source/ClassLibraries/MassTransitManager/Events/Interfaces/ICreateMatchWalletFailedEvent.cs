using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ICreateMatchWalletFailedEvent : CorrelatedBy<Guid>
{
    public string ErrorMessage { get; }
}