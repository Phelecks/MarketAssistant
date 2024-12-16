using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IFinancialAccountCreatedEvent : CorrelatedBy<Guid>
{

    long AccountId { get; }
}