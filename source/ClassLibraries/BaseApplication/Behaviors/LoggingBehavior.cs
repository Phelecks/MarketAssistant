using IdentityHelper.Helpers;
using LoggerService.Helpers;
using MediatR.Interfaces;
using Microsoft.Extensions.Logging;

namespace BaseApplication.Behavior;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger, IIdentityHelper identityHelper) 
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger _logger = logger;
    private readonly IIdentityHelper _identityHelper = identityHelper;

    public async Task<TResponse> HandleAsync(TRequest request, Func<CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _identityHelper.GetUserIdentity();
        string username = _identityHelper.GetUserName();

        _ = Task.Run(() => _logger.LogInformation(
            eventId: EventTool.GetEventInformation(eventType: EventType.Debug, eventName: "Application Request"),
            "Application request, Name: {@requestName}, UserId: {@userId}, username: {@username}, request: {@request}",
            requestName, userId, username, request), cancellationToken);

        return await next(cancellationToken);
    }
}
