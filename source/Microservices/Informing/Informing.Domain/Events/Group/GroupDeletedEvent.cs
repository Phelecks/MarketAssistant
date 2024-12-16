using BaseDomain.Common;

namespace Informing.Domain.Events.Group;

public class GroupDeletedEvent : BaseEvent
{
    public GroupDeletedEvent(Entities.Group item)
    {
        this.item = item;
    }

    public Entities.Group item { get; }
}
