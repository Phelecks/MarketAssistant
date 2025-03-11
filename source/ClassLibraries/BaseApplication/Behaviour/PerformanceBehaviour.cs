using System.Diagnostics;
using IdentityHelper.Helpers;
using LoggerService.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BaseApplication.Behaviour;

public class PerformanceBehaviour<TRequest, TResponse>(
    ILogger<PerformanceBehaviour<TRequest, TResponse>> logger,
    IIdentityHelper identityHelper) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _timer = new Stopwatch();
    private readonly ILogger<PerformanceBehaviour<TRequest, TResponse>> _logger = logger;
    private readonly IIdentityHelper _identityHelper = identityHelper;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

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
