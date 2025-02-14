using CacheManager.Interfaces;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

namespace CacheManager.Services;

public class DistributedLockService : IDistributedLockService
{
    private readonly RedLockFactory _redLockFactory;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public DistributedLockService(IConnectionMultiplexer connectionMultiplexer, RedLockFactory redLockFactory)
    {
        _connectionMultiplexer = connectionMultiplexer;
        var endPoints = _connectionMultiplexer.GetEndPoints();
        List<RedLockEndPoint> redLockEndPoints = [.. endPoints];

        _redLockFactory = RedLockFactory.Create(redLockEndPoints);
    }

    public async Task RunWithLockAsync(Func<Task> func, string key, int expiryInSecond = 30, int waitInSecond = 10, int retryInSecond = 1, CancellationToken cancellationToken = default)
    {
        // blocks until acquired or 'wait' timeout
        await using var redLock = await _redLockFactory.CreateLockAsync(key, TimeSpan.FromSeconds(expiryInSecond), TimeSpan.FromSeconds(waitInSecond),
            TimeSpan.FromSeconds(retryInSecond), cancellationToken);

        if (redLock.IsAcquired)
            await func();
    }

    public async Task RunWithLockAsync(Task task, string key, int expiryInSecond = 30, int waitInSecond = 10, int retryInSecond = 1, CancellationToken cancellationToken = default)
    {
        // blocks until acquired or 'wait' timeout
        await using var redLock = await _redLockFactory.CreateLockAsync(key, TimeSpan.FromSeconds(expiryInSecond), TimeSpan.FromSeconds(waitInSecond),
            TimeSpan.FromSeconds(retryInSecond), cancellationToken);

        if (redLock.IsAcquired)
            await task;
    }
}
