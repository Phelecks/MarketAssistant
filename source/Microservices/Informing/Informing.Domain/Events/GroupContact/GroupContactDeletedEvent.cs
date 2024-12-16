using BaseDomain.Common;

namespace Informing.Domain.Events.GroupContact;

public class GroupContactDeletedEvent : BaseEvent
{
    public GroupContactDeletedEvent(Entities.GroupContact item)
    {
        this.item = item;
    }

    public Entities.GroupContact item { get; }
}
