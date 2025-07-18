using MediatR.Interfaces;

namespace BlockProcessor.Domain.Events.RpcUrl;

public class RpcUrlDeletedEvent(Entities.RpcUrl entity) : INotification
{
    public Entities.RpcUrl Entity { get; } = entity;
}
