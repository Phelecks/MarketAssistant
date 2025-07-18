using MediatR.Interfaces;

namespace Informing.Domain.Events.Information;

public class InformationUpdatedEvent : INotification
{
    public InformationUpdatedEvent(Entities.Information item)
    {
        this.item = item;
    }

    public Entities.Information item { get; }
}
