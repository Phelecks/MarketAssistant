using System.Diagnostics;
using IdentityHelper.Helpers;
using LoggerService.Helpers;
using MediatR.Attributes;
using MediatR.Interfaces;
using Microsoft.Extensions.Logging;

namespace BaseApplication.Behavior;

[BehaviorOrder(4)]
public class PerformanceBehavior<TRequest, TResponse>(
    ILogger<PerformanceBehavior<TRequest, TResponse>> logger,
    IIdentityHelper identityHelper) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _timer = new();
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger = logger;
    private readonly IIdentityHelper _identityHelper = identityHelper;

    public async Task<TResponse> HandleAsync(TRequest request, Func<CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next(cancellationToken);

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _identityHelper.GetUserIdentity();
            var username = _identityHelper.GetUserName();

            _ = Task.Run(() => _logger.LogWarning(
                eventId: EventTool.GetEventInformation(eventType: EventType.Performance, eventName: "Application Long Running Request"),
                "Application Long Running Request, Name: {@requestName}, UserId: {@userId}, Username: {@username}, request: {@request}, elapsedMilliseconds: {@elapsedMilliseconds}",
                requestName, userId, username, request, elapsedMilliseconds), cancellationToken);
        }

        return response;
    }
}
