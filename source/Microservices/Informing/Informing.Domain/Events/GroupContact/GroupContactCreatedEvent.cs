using BaseDomain.Common;

namespace Informing.Domain.Events.GroupContact;

public class GroupContactCreatedEvent : BaseEvent
{
    public GroupContactCreatedEvent(Entities.GroupContact item)
    {
        this.item = item;
    }

    public Entities.GroupContact item { get; }
}
