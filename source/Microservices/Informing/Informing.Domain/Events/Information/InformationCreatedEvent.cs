using BaseDomain.Common;

namespace Informing.Domain.Events.Information;

public class InformationCreatedEvent : BaseEvent
{
    public InformationCreatedEvent(Entities.Information item)
    {
        this.item = item;
    }

    public Entities.Information item { get; }
}
