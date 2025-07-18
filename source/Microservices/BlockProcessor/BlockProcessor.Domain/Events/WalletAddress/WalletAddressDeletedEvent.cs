using MediatR.Interfaces;

namespace BlockProcessor.Domain.Events.WalletAddress;

public class WalletAddressDeletedEvent(Entities.WalletAddress entity) : INotification
{
    public Entities.WalletAddress Entity { get; } = entity;
}
