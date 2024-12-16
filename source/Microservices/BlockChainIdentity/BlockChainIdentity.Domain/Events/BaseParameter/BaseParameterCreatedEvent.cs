using BaseDomain.Common;

namespace BlockChainIdentity.Domain.Events.BaseParameter;

public class BaseParameterCreatedEvent : BaseEvent
{
    public BaseParameterCreatedEvent(Entities.BaseParameter item)
    {
        this.item = item;
    }

    public Entities.BaseParameter item { get; }
}
