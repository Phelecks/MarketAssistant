using BaseDomain.Common;

namespace BlockProcessor.Domain.Events.WalletAddress;

public class WalletAddressDeletedEvent(Entities.WalletAddress entity) : BaseEvent
{
    public Entities.WalletAddress Entity { get; } = entity;
}
