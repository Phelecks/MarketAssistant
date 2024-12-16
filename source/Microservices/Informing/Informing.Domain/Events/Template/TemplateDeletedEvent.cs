using BaseDomain.Common;

namespace Informing.Domain.Events.Template;

public class TemplateDeletedEvent : BaseEvent
{
    public TemplateDeletedEvent(Entities.Template item)
    {
        this.item = item;
    }

    public Entities.Template item { get; }
}
