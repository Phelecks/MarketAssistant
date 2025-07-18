using MediatR.Interfaces;

namespace Informing.Domain.Events.Template;

public class TemplateDeletedEvent : INotification
{
    public TemplateDeletedEvent(Entities.Template item)
    {
        this.item = item;
    }

    public Entities.Template item { get; }
}
