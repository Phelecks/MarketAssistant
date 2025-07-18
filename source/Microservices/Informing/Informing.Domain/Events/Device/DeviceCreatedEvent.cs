using MediatR.Interfaces;

namespace Informing.Domain.Events.Device;

public class DeviceCreatedEvent : INotification
{
    public DeviceCreatedEvent(Entities.Device item)
    {
        this.item = item;
    }

    public Entities.Device item { get; }
}
