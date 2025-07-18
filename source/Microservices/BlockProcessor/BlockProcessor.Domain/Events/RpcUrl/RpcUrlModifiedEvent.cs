using MediatR.Interfaces;

namespace BlockProcessor.Domain.Events.RpcUrl;

public class RpcUrlModifiedEvent(Entities.RpcUrl entity) : INotification
{
    public Entities.RpcUrl Entity { get; } = entity;
}
