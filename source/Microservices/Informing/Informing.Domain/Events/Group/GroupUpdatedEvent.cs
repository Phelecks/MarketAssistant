using BaseDomain.Common;

namespace Informing.Domain.Events.Group;

public class GroupUpdatedEvent : BaseEvent
{
    public GroupUpdatedEvent(Entities.Group item)
    {
        this.item = item;
    }

    public Entities.Group item { get; }
}
