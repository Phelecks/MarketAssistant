using BaseDomain.Common;

namespace Informing.Domain.Events.Contact;

public class ContactUpdatedEvent : BaseEvent
{
    public ContactUpdatedEvent(Entities.Contact item)
    {
        this.item = item;
    }

    public Entities.Contact item { get; }
}
