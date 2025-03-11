using IdentityHelper.Helpers;
using LoggerService.Helpers;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace BaseApplication.Behaviour;

public class LoggingBehaviour<TRequest>(ILogger<LoggingBehaviour<TRequest>> logger, IIdentityHelper identityHelper) : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger = logger;
    private readonly IIdentityHelper _identityHelper = identityHelper;

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _identityHelper.GetUserIdentity();
        string username = _identityHelper.GetUserName();

        _ = Task.Run(() => _logger.LogInformation(
            eventId: EventTool.GetEventInformation(eventType: EventType.General, eventName: "Application Request"),
            "Application request, Name: {@requestName}, UserId: {@userId}, username: {@username}, request: {@request}",
            requestName, userId, username, request), cancellationToken);

        await Task.CompletedTask;
    }
}
