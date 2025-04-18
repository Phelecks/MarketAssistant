﻿using BaseDomain.Enums;
using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class ContractBaseParameterUpdatedEvent : IBaseParameterUpdatedEvent
{
    public ContractBaseParameterUpdatedEvent(BaseParameterCategory category, BaseParameterField field, string value, long kernelBaseParameterId)
    {
        Category = category;
        Field = field;
        Value = value;
        KernelBaseParameterId = kernelBaseParameterId;
    }

    public BaseParameterCategory Category { get; }
    public BaseParameterField Field { get; }
    public string Value { get; }
    public long KernelBaseParameterId { get; }
}