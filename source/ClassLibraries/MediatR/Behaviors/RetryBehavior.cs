using MediatR.Attributes;
using MediatR.Interfaces;
using LoggerService.Helpers;
using Microsoft.Extensions.Logging;

namespace MediatR.Behaviors;

[BehaviorOrder(3)]
public class RetryBehavior<TRequest, TResponse>(ILogger<RetryBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly int _retryCount = 3;
    private readonly ILogger<RetryBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> HandleAsync(TRequest request, Func<CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken)
    {
        for (int attempt = 1; attempt <= _retryCount; attempt++)
        {
            try
            {
                return await next(cancellationToken);
            }
            catch (Exception exception) when (attempt < _retryCount)
            {
                _ = Task.Run(() => _logger.LogError(
                    eventId: EventTool.GetEventInformation(eventType: EventType.Exception, eventName: "Application Retry Exception"),
                    exception: exception,
                    message: "Application Retry Exception, {@attempt} attempt, name: {@requestName}, request: {@request},",
                    attempt, typeof(TRequest).Name, request), cancellationToken);
            }
        }

        _ = Task.Run(() => _logger.LogError(
                    eventId: EventTool.GetEventInformation(eventType: EventType.Exception, eventName: "Application Retry Exception"),
                    message: "Application Retry Exception, name: {@requestName}, request: {@request},",
                    typeof(TRequest).Name, request), cancellationToken);

        throw new Exception($"All retries failed for {typeof(TRequest).Name}");
    }
}
