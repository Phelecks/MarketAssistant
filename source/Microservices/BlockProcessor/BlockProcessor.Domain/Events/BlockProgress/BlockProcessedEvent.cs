using BaseDomain.Common;

namespace BlockProcessor.Domain.Events.BlockProgress;

public class BlockProcessedEvent(Entities.BlockProgress entity) : BaseEvent
{
    public Entities.BlockProgress Entity { get; } = entity;
}