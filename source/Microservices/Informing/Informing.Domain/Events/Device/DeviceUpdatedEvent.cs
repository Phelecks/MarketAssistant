using MediatR.Interfaces;

namespace Informing.Domain.Events.Device;

public class DeviceUpdatedEvent : INotification
{
    public DeviceUpdatedEvent(Entities.Device item)
    {
        this.item = item;
    }

    public Entities.Device item { get; }
}
