using BaseDomain.Common;

namespace LogProcessor.Domain.Events.RpcUrl;

public class RpcUrlCreatedEvent(Entities.RpcUrl entity) : BaseEvent
{
    public Entities.RpcUrl Entity { get; } = entity;
}
