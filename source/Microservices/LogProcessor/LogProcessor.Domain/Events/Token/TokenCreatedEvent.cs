using BaseDomain.Common;

namespace LogProcessor.Domain.Events.Token;

public class TokenCreatedEvent(Entities.Token entity) : BaseEvent
{
    public Entities.Token Entity { get; } = entity;
}
