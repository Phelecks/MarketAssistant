using BaseDomain.Common;

namespace BlockChainIdentity.Domain.Events.BaseParameter;

public class BaseParameterDeletedEvent : BaseEvent
{
    public BaseParameterDeletedEvent(Entities.BaseParameter item)
    {
        this.item = item;
    }

    public Entities.BaseParameter item { get; }
}
