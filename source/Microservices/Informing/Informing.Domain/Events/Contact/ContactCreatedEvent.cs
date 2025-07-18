using MediatR.Interfaces;

namespace Informing.Domain.Events.Contact;

public class ContactCreatedEvent : INotification
{
    public ContactCreatedEvent(Entities.Contact item)
    {
        this.item = item;
    }

    public Entities.Contact item { get; }
}
