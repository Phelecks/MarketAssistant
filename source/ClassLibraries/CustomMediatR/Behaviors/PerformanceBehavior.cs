using System.Diagnostics;
using CustomMediatR.Attributes;
using CustomMediatR.Interfaces;
using LoggerService.Helpers;
using Microsoft.Extensions.Logging;

namespace CustomMediatR.Behaviors;

[BehaviorOrder(4)]
public class PerformanceBehavior<TRequest, TResponse>(ILogger<PerformanceBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger = logger;
    private readonly Stopwatch _timer = new();

    public async Task<TResponse> HandleAsync(TRequest request, Func<CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        //Log the request started
        _ = Task.Run(() => _logger.LogInformation(
            eventId: EventTool.GetEventInformation(eventType: EventType.Debug, eventName: "Application Request"),
            "Application request started, request: {@request}", request), cancellationToken);

        var response = await next(cancellationToken);

        //Log the request started
        _ = Task.Run(() => _logger.LogInformation(
            eventId: EventTool.GetEventInformation(eventType: EventType.Debug, eventName: "Application Request"),
            "Application request finished, request: {@request}", request), cancellationToken);


        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
            _ = Task.Run(() => _logger.LogWarning(
                eventId: EventTool.GetEventInformation(eventType: EventType.Performance, eventName: "Application Long Running"),
                "Application Long Running, name: {@requestName}, request: {@request}, elapsedMilliseconds: {@elapsedMilliseconds}",
                typeof(TRequest).Name, request, elapsedMilliseconds), cancellationToken);

        return response;
    }
}
