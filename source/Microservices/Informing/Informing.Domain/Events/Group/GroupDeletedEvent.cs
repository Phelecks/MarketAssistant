using MediatR.Interfaces;

namespace Informing.Domain.Events.Group;

public class GroupDeletedEvent : INotification
{
    public GroupDeletedEvent(Entities.Group item)
    {
        this.item = item;
    }

    public Entities.Group item { get; }
}
