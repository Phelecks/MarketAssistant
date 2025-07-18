using LoggerService.Helpers;
using MediatR.Attributes;
using MediatR.Interfaces;
using Microsoft.Extensions.Logging;

namespace BaseApplication.Behavior;

[BehaviorOrder(1)]
public class UnhandledExceptionBehavior<TRequest, TResponse>(ILogger<TRequest> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger = logger;

    public async Task<TResponse> HandleAsync(TRequest request, Func<CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next(cancellationToken);
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            _ = Task.Run(() => _logger.LogError(
                eventId: EventTool.GetEventInformation(eventType: EventType.Exception, eventName: "Exception"),
                exception: ex, message: "Application unhandled exception, Name: {@requestName}, request: {@request}, errorMessage: {@errorMessage}",
                requestName, request, ex.Message), cancellationToken);

            throw;
        }
    }
}
