using MediatR.Interfaces;

namespace Informing.Domain.Events.Template;

public class TemplateUpdatedEvent : INotification
{
    public TemplateUpdatedEvent(Entities.Template item)
    {
        this.item = item;
    }

    public Entities.Template item { get; }
}
