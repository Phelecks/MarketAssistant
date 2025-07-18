using MediatR.Interfaces;

namespace BlockChainIdentity.Domain.Events.Wallet;

public class WalletSignedOutEvent : INotification
{
    public WalletSignedOutEvent(Entities.Wallet item)
    {
        Item = item;
    }

    public Entities.Wallet Item { get; }
}
