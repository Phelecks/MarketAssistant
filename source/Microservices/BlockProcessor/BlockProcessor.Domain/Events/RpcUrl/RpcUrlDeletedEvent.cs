using BaseDomain.Common;

namespace BlockProcessor.Domain.Events.RpcUrl;

public class RpcUrlDeletedEvent(Entities.RpcUrl entity) : BaseEvent
{
    public Entities.RpcUrl Entity { get; } = entity;
}
