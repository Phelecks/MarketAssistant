using BaseDomain.Common;

namespace Informing.Domain.Events.Contact;

public class ContactDeletedEvent : BaseEvent
{
    public ContactDeletedEvent(Entities.Contact item)
    {
        this.item = item;
    }

    public Entities.Contact item { get; }
}
