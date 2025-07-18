using MediatR.Interfaces;

namespace Informing.Domain.Events.GroupContact;

public class GroupContactDeletedEvent : INotification
{
    public GroupContactDeletedEvent(Entities.GroupContact item)
    {
        this.item = item;
    }

    public Entities.GroupContact item { get; }
}
