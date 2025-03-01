using BaseDomain.Common;

namespace BlockProcessor.Domain.Events.Transfer;

public class TransferMarkedAsConfirmedEvent(Entities.Transfer entity) : BaseEvent
{
    public Entities.Transfer Entity { get; } = entity;
}
