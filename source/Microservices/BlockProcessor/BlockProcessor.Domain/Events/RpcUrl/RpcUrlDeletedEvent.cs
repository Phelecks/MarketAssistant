using BaseDomain.Common;

namespace BlockProcessor.Domain.Events.RpcUrl;

public class RpcUrlDeletedEvent(Entities.RpcUrl item) : BaseEvent
{
    public Entities.RpcUrl RpcUrl { get; } = item;
}
