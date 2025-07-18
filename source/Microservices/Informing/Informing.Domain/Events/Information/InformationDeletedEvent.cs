using MediatR.Interfaces;

namespace Informing.Domain.Events.Information;

public class InformationDeletedEvent : INotification
{
    public InformationDeletedEvent(Entities.Information item)
    {
        this.item = item;
    }

    public Entities.Information item { get; }
}
