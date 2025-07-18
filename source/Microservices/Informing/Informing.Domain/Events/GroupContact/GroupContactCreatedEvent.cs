using MediatR.Interfaces;

namespace Informing.Domain.Events.GroupContact;

public class GroupContactCreatedEvent : INotification
{
    public GroupContactCreatedEvent(Entities.GroupContact item)
    {
        this.item = item;
    }

    public Entities.GroupContact item { get; }
}
