using BaseDomain.Common;

namespace BlockChainIdentity.Domain.Events.Wallet;

public class WalletSignedOutEvent : BaseEvent
{
    public WalletSignedOutEvent(Entities.Wallet item)
    {
        Item = item;
    }

    public Entities.Wallet Item { get; }
}
