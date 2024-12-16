using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;

namespace ExecutorManager;

public static class ConfigureServices
{
    public static void AddExecutorManagerDependencyInjections(this IServiceCollection services)
    {
        services.AddResiliencePipeline(Helpers.PipelineHelper.RetryEverythingForever, builder =>
        {
            builder
                .AddRetry(new RetryStrategyOptions
                {
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,  // Adds a random factor to the delay
                    MaxRetryAttempts = int.MaxValue,
                    Delay = TimeSpan.FromSeconds(1),
                    MaxDelay = TimeSpan.FromSeconds(10),
                });
        });

        services.AddResiliencePipeline(Helpers.PipelineHelper.RetryEverythingFiveTimes, builder =>
        {
            builder
                .AddRetry(new RetryStrategyOptions
                {
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,  // Adds a random factor to the delay
                    MaxRetryAttempts = 5,
                    Delay = TimeSpan.FromSeconds(1),
                    MaxDelay = TimeSpan.FromSeconds(10),
                });
        });
    }
}