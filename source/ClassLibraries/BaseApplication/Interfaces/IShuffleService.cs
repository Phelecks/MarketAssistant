namespace BaseApplication.Interfaces;

public interface IShuffleService<T>
{
    T Shuffle(List<T> items);
}