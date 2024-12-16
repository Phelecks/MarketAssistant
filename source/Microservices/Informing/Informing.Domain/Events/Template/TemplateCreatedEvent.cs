using BaseDomain.Common;

namespace Informing.Domain.Events.Template;

public class TemplateCreatedEvent : BaseEvent
{
    public TemplateCreatedEvent(Entities.Template item)
    {
        this.item = item;
    }

    public Entities.Template item { get; }
}
