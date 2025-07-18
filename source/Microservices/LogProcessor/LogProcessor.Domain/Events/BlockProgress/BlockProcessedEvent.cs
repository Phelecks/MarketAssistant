using MediatR.Interfaces;

namespace LogProcessor.Domain.Events.BlockProgress;

public class BlockProcessedEvent(Entities.BlockProgress entity) : INotification
{
    public Entities.BlockProgress Entity { get; } = entity;
}