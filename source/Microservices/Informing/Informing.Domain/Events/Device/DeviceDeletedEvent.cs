using BaseDomain.Common;

namespace Informing.Domain.Events.Device;

public class DeviceDeletedEvent : BaseEvent
{
    public DeviceDeletedEvent(Entities.Device item)
    {
        this.item = item;
    }

    public Entities.Device item { get; }
}
