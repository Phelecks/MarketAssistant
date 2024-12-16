using IdentityHelper.Helpers;
using LoggerService.Helpers;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace BaseApplication.Behaviour;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly IIdentityHelper _identityHelper;
    //private readonly IIdentityService _identityService;

    public LoggingBehaviour(ILogger<TRequest> logger, IIdentityHelper identityHelper/*, IIdentityService identityService*/)
    {
        _logger = logger;
        _identityHelper = identityHelper;
        //_identityService = identityService;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _identityHelper.GetUserIdentity();
        string username = _identityHelper.GetUserName();

        //if (!string.IsNullOrEmpty(userId))
        //{
        //    userName = await _identityService.GetUserNameAsync(userId);
        //}

        _ = Task.Run(() => _logger.LogInformation(
            eventId: EventTool.GetEventInformation(eventType: EventType.General, eventName: "Application Request"),
            "Application request, Name: {@requestName}, UserId: {@userId}, username: {@username}, request: {@request}",
            requestName, userId, username, request), cancellationToken);
    }
}
