using MediatR.Interfaces;

namespace Informing.Domain.Events.Group;

public class GroupUpdatedEvent : INotification
{
    public GroupUpdatedEvent(Entities.Group item)
    {
        this.item = item;
    }

    public Entities.Group item { get; }
}
