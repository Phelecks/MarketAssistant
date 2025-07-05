using Microsoft.Extensions.DependencyInjection;
using CustomMediatR.Interfaces;

namespace CustomMediatR.Services;

public class RequestDispatcher(IServiceProvider provider)
{
    private readonly IServiceProvider _provider = provider;

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        var handler = _provider.GetRequiredService(handlerType);

        var behaviors = _provider
            .GetServices(typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse)))
            .Cast<object>()
            .Reverse()
            .ToList();

        Func<CancellationToken, Task<TResponse>> handlerDelegate = ct =>
            ((dynamic)handler).HandleAsync((dynamic)request, ct);

        foreach (var behavior in behaviors)
        {
            var next = handlerDelegate;
            handlerDelegate = ct =>
                ((dynamic)behavior).HandleAsync((dynamic)request, next, ct);
        }

        return await handlerDelegate(cancellationToken);
    }

    public async Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        var handlers = _provider.GetServices<INotificationHandler<TNotification>>();
        var tasks = handlers.Select(h => h.HandleAsync(notification, cancellationToken));
        await Task.WhenAll(tasks);
    }
}
