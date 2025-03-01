﻿using BaseDomain.Common;

namespace BlockProcessor.Domain.Events.RpcUrl;

public class RpcUrlModifiedEvent(Entities.RpcUrl entity) : BaseEvent
{
    public Entities.RpcUrl RpcUrl { get; } = entity;
}
