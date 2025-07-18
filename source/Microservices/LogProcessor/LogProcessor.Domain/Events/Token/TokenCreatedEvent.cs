using MediatR.Interfaces;

namespace LogProcessor.Domain.Events.Token;

public class TokenCreatedEvent(Entities.Token entity) : INotification
{
    public Entities.Token Entity { get; } = entity;
}
