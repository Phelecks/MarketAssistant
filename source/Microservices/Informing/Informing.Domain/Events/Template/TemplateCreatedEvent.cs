using MediatR.Interfaces;

namespace Informing.Domain.Events.Template;

public class TemplateCreatedEvent : INotification
{
    public TemplateCreatedEvent(Entities.Template item)
    {
        this.item = item;
    }

    public Entities.Template item { get; }
}
