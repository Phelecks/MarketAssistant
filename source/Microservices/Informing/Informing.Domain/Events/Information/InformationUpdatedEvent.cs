using BaseDomain.Common;

namespace Informing.Domain.Events.Information;

public class InformationUpdatedEvent : BaseEvent
{
    public InformationUpdatedEvent(Entities.Information item)
    {
        this.item = item;
    }

    public Entities.Information item { get; }
}
