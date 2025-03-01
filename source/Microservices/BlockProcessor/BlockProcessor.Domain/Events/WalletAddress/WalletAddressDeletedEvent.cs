using BaseDomain.Common;

namespace BlockProcessor.Domain.Events.WalletAddress;

public class WalletAddressDeletedEvent(Entities.WalletAddress item) : BaseEvent
{
    public Entities.WalletAddress WalletAddress { get; } = item;
}
