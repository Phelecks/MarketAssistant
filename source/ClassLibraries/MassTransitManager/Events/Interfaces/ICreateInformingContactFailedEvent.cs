using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ICreateInformingContactFailedEvent : CorrelatedBy<Guid>
{

    string ErrorMessage { get; }
}