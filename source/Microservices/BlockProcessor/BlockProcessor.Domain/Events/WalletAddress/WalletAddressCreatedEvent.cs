﻿using BaseDomain.Common;

namespace BlockProcessor.Domain.Events.WalletAddress;

public class WalletAddressCreatedEvent(Entities.WalletAddress entity) : BaseEvent
{
    public Entities.WalletAddress WalletAddress { get; } = entity;
}
