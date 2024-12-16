using BaseDomain.Common;

namespace Informing.Domain.Events.BaseParameter;

public class BaseParameterUpdatedEvent : BaseEvent
{
    public BaseParameterUpdatedEvent(Entities.BaseParameter item)
    {
        this.item = item;
    }

    public Entities.BaseParameter item { get; }
}
