using BaseDomain.Common;

namespace Informing.Domain.Events.Information;

public class SystemErrorSentEvent : BaseEvent
{
    public SystemErrorSentEvent(Entities.Information item)
    {
        this.item = item;
    }

    public Entities.Information item { get; }
}
