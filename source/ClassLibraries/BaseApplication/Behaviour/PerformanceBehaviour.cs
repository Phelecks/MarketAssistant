using System.Diagnostics;
using BaseApplication.Interfaces;
using IdentityHelper.Helpers;
using LoggerService.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BaseApplication.Behaviour;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;
    private readonly IIdentityHelper _identityHelper;
    //private readonly IIdentityService _identityService;

    public PerformanceBehaviour(
        ILogger<TRequest> logger,
        IIdentityHelper identityHelper/*, IIdentityService identityService*/)
    {
        _timer = new Stopwatch();

        _logger = logger;
        _identityHelper = identityHelper;
        //_identityService = identityService;
    }

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

            //if (!string.IsNullOrEmpty(userId))
            //{
            //    userName = await _identityService.GetUserNameAsync(userId);
            //}

            _ = Task.Run(() => _logger.LogWarning(
                eventId: EventTool.GetEventInformation(eventType: EventType.Performance, eventName: "Application Long Running Request"),
                "Application Long Running Request, Name: {@requestName}, UserId: {@userId}, Username: {@username}, request: {@request}, elapsedMilliseconds: {@elapsedMilliseconds}",
                requestName, userId, username, request, elapsedMilliseconds), cancellationToken);
        }

        return response;
    }
}
