using BaseDomain.Common;

namespace LogProcessor.Domain.Events.Transfer;

public class TransferInitiatedEvent(Entities.Transfer entity) : BaseEvent
{
    public Entities.Transfer Entity { get; } = entity;
}
