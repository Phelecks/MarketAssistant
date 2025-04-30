using BaseDomain.Common;

namespace LogProcessor.Domain.Events.Transfer;

public class TransferMarkedAsFailedEvent(Entities.Transfer entity, string errorMessage) : BaseEvent
{
    public Entities.Transfer Entity { get; } = entity;
    public string ErrorMessage { get; } = errorMessage;
}
