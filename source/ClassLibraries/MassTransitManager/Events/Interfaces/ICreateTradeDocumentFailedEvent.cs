using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ICreateTradeDocumentFailedEvent : CorrelatedBy<Guid>
{
    string ErrorMessage { get; }
}