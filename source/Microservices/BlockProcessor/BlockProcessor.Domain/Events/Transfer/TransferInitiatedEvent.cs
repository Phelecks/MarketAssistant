﻿using BaseDomain.Common;

namespace BlockProcessor.Domain.Events.Transfer;

public class TransferInitiatedEvent(Entities.Transfer entity) : BaseEvent
{
    public Entities.Transfer Transfer { get; } = entity;
}
