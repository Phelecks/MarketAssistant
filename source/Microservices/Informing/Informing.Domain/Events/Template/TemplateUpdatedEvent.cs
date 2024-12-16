using BaseDomain.Common;

namespace Informing.Domain.Events.Template;

public class TemplateUpdatedEvent : BaseEvent
{
    public TemplateUpdatedEvent(Entities.Template item)
    {
        this.item = item;
    }

    public Entities.Template item { get; }
}
