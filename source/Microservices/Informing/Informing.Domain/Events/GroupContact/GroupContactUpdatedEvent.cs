using MediatR.Interfaces;

namespace Informing.Domain.Events.GroupContact;

public class GroupContactUpdatedEvent : INotification
{
    public GroupContactUpdatedEvent(Entities.GroupContact item)
    {
        this.item = item;
    }

    public Entities.GroupContact item { get; }
}
