using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IInformingContactCreatedEvent : CorrelatedBy<Guid>
{
    long ContactId { get; }
}