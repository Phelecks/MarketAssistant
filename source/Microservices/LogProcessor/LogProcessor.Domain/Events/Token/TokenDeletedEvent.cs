using MediatR.Interfaces;

namespace LogProcessor.Domain.Events.Token;

public class TokenDeletedEvent(Entities.Token entity) : INotification
{
    public Entities.Token Entity { get; } = entity;
}
