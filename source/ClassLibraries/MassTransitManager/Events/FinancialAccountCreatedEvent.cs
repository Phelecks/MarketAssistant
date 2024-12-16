using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class FinancialAccountCreatedEvent : IFinancialAccountCreatedEvent
{
    public Guid CorrelationId { get; }

    public long AccountId { get; }

    public FinancialAccountCreatedEvent(Guid correlationId, long accountId)
    {
        CorrelationId = correlationId;
        AccountId = accountId;
    }
}