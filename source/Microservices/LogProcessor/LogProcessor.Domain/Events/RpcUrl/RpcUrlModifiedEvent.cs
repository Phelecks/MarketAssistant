using MediatR.Interfaces;

namespace LogProcessor.Domain.Events.RpcUrl;

public class RpcUrlModifiedEvent(Entities.RpcUrl entity) : INotification
{
    public Entities.RpcUrl Entity { get; } = entity;
}
