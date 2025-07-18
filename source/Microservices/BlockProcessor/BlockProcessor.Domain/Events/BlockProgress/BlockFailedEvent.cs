using MediatR.Interfaces;

namespace BlockProcessor.Domain.Events.BlockProgress;

public class BlockFailedEvent(Entities.BlockProgress entity) : INotification
{
    public Entities.BlockProgress Entity { get; } = entity;
}