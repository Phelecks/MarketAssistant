namespace MediatR.Helpers;

public sealed class Unit
{
    public static readonly Unit Value = new();
    private Unit() { }
}