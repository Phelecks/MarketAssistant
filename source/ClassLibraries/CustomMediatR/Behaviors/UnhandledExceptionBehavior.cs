using CustomMediatR.Attributes;
using CustomMediatR.Interfaces;
using LoggerService.Helpers;
using Microsoft.Extensions.Logging;

namespace CustomMediatR.Behaviors;

[BehaviorOrder(1)]
public class UnhandledExceptionBehavior<TRequest, TResponse>(ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> _logger = logger;
    public async Task<TResponse> HandleAsync(TRequest request, Func<CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next(cancellationToken);
        }
        catch (Exception ex)
        {
            _ = Task.Run(() => _logger.LogError(
                eventId: EventTool.GetEventInformation(eventType: EventType.Exception, eventName: "Exception"),
                exception: ex, message: "Application unhandled exception, Name: {@requestName}, request: {@request}, errorMessage: {@errorMessage}",
                typeof(TRequest).Name, request, ex.Message), cancellationToken);

            throw;
        }
    }
}
