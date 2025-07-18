namespace MediatR.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class BehaviorOrderAttribute(int order) : Attribute
{
    public int Order { get; } = order;
}