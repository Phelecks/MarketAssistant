using MediatR.Interfaces;

namespace Informing.Domain.Events.Contact;

public class ContactUpdatedEvent : INotification
{
    public ContactUpdatedEvent(Entities.Contact item)
    {
        this.item = item;
    }

    public Entities.Contact item { get; }
}
