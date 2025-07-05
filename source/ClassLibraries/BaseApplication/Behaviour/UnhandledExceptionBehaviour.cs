using LoggerService.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BaseApplication.Behaviour;

public class UnhandledExceptionBehaviour<TRequest, TResponse>(ILogger<TRequest> logger) : 
    IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
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
