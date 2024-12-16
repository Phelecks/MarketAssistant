using BaseDomain.Common;

namespace Informing.Domain.Events.Device;

public class DeviceUpdatedEvent : BaseEvent
{
    public DeviceUpdatedEvent(Entities.Device item)
    {
        this.item = item;
    }

    public Entities.Device item { get; }
}
