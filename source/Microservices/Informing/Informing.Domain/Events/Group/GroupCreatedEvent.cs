using BaseDomain.Common;

namespace Informing.Domain.Events.Group;

public class GroupCreatedEvent : BaseEvent
{
    public GroupCreatedEvent(Entities.Group item)
    {
        this.item = item;
    }

    public Entities.Group item { get; }
}
