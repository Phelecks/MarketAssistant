using MediatR.Interfaces;

namespace Informing.Domain.Events.Contact;

public class ContactDeletedEvent : INotification
{
    public ContactDeletedEvent(Entities.Contact item)
    {
        this.item = item;
    }

    public Entities.Contact item { get; }
}
