using BaseDomain.Enums;

namespace MassTransitManager.Events.Interfaces;

public interface IBaseParameterUpdatedEvent
{
    /// <summary>
    /// Category 
    /// </summary>
    BaseParameterCategory Category { get; }

    /// <summary>
    /// Field
    /// </summary>
    BaseParameterField Field { get; }

    /// <summary>
    /// Value
    /// </summary>
    string Value { get; }

    /// <summary>
    /// Kernel Base Parameter Id 
    /// </summary>
    long KernelBaseParameterId { get; }
}