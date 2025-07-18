using MediatR.Interfaces;

namespace BlockProcessor.Domain.Events.Transfer;

public class TransferMarkedAsFailedEvent(Entities.Transfer entity, string errorMessage) : INotification
{
    public Entities.Transfer Entity { get; } = entity;
    public string ErrorMessage { get; } = errorMessage;
}
