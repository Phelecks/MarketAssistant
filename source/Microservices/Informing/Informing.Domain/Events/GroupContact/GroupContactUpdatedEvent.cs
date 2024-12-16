using BaseDomain.Common;

namespace Informing.Domain.Events.GroupContact;

public class GroupContactUpdatedEvent : BaseEvent
{
    public GroupContactUpdatedEvent(Entities.GroupContact item)
    {
        this.item = item;
    }

    public Entities.GroupContact item { get; }
}
