using BaseApplication.Interfaces;

namespace BaseInfrastructure.Services;

public class ShuffleService<T> : IShuffleService<T>
{
    private readonly Random _random = new();

    public T Shuffle(List<T> items)
    {
        var span = new Span<T>([.. items]);
        _random.Shuffle(span);
        return span.ToArray()[0];
    }
}