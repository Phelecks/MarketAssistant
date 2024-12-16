using BaseDomain.Common;

namespace Informing.Domain.Events.Contact;

public class ContactCreatedEvent : BaseEvent
{
    public ContactCreatedEvent(Entities.Contact item)
    {
        this.item = item;
    }

    public Entities.Contact item { get; }
}
