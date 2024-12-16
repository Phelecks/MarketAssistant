using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ICreateFinancialAccountFailedEvent : CorrelatedBy<Guid>
{
    string ErrorMessage { get; }
}