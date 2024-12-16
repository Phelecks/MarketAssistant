using BaseDomain.Common;

namespace Informing.Domain.Events.Information;

public class InformationDeletedEvent : BaseEvent
{
    public InformationDeletedEvent(Entities.Information item)
    {
        this.item = item;
    }

    public Entities.Information item { get; }
}
