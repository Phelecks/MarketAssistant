using BaseDomain.Common;

namespace Informing.Domain.Events.Device;

public class DeviceCreatedEvent : BaseEvent
{
    public DeviceCreatedEvent(Entities.Device item)
    {
        this.item = item;
    }

    public Entities.Device item { get; }
}
