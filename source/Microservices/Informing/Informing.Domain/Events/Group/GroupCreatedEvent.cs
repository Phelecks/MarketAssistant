using MediatR.Interfaces;

namespace Informing.Domain.Events.Group;

public class GroupCreatedEvent : INotification
{
    public GroupCreatedEvent(Entities.Group item)
    {
        this.item = item;
    }

    public Entities.Group item { get; }
}
