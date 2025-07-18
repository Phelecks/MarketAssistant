using MediatR.Interfaces;

namespace BlockProcessor.Domain.Events.Transfer;

public class TransferMarkedAsConfirmedEvent(Entities.Transfer entity) : INotification
{
    public Entities.Transfer Entity { get; } = entity;
}
