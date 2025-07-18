using MediatR.Interfaces;

namespace Informing.Domain.Events.Device;

public class DeviceDeletedEvent : INotification
{
    public DeviceDeletedEvent(Entities.Device item)
    {
        this.item = item;
    }

    public Entities.Device item { get; }
}
