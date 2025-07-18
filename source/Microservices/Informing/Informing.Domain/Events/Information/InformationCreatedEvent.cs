using MediatR.Interfaces;

namespace Informing.Domain.Events.Information;

public class InformationCreatedEvent : INotification
{
    public InformationCreatedEvent(Entities.Information item)
    {
        this.item = item;
    }

    public Entities.Information item { get; }
}
