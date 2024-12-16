using BaseDomain.Common;

namespace BlockChainIdentity.Domain.Events.Wallet;

public class WalletCreatedEvent : BaseEvent
{
    public WalletCreatedEvent(Entities.Wallet item, string clientId)
    {
        Item = item;
        ClientId = clientId;
    }

    public Entities.Wallet Item { get; }

    public string ClientId { get; set; }
}
