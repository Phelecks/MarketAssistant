using BaseDomain.Common;

namespace LogProcessor.Domain.Events.BlockProgress;

public class BlockFailedEvent(Entities.BlockProgress entity) : BaseEvent
{
    public Entities.BlockProgress Entity { get; } = entity;
}