namespace CacheManager.Interfaces;

public interface IDistributedLockService
{
    Task RunWithLockAsync(Func<Task> func, string key, int expiryInSecond = 30, int waitInSecond = 10, int retryInSecond = 1, CancellationToken cancellationToken = default);
    Task RunWithLockAsync(Task task, string key, int expiryInSecond = 30, int waitInSecond = 10, int retryInSecond = 1, CancellationToken cancellationToken = default);
}
