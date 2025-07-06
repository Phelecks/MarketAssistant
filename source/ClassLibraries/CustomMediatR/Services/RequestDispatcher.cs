using Microsoft.Extensions.DependencyInjection;
using CustomMediatR.Interfaces;
using CustomMediatR.Attributes;
using System.Reflection;

namespace CustomMediatR.Services;

public class RequestDispatcher(IServiceProvider provider) : IRequestDistpacher
{
    private readonly IServiceProvider _provider = provider;

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        var handler = _provider.GetRequiredService(handlerType);

        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));
        var rawBehaviors = _provider.GetServices(behaviorType).ToList();

        var orderedBehaviors = rawBehaviors
            .Select((b, index) => new
            {
                Behavior = b,
                Order = b?.GetType().GetCustomAttribute<BehaviorOrderAttribute>()?.Order,
                RegistrationIndex = index
            })
            .OrderBy(x => x.Order ?? int.MaxValue)   // sort by attribute first
            .ThenBy(x => x.RegistrationIndex)        // fallback: registration order
            .Select(x => x.Behavior)
            .Reverse()                               // reverse for correct middleware-style chaining
            .ToList();

        Func<CancellationToken, Task<TResponse>> handlerDelegate = ct =>
            ((dynamic)handler).HandleAsync((dynamic)request, ct);

        foreach (var behavior in orderedBehaviors)
        {
            if (behavior is null) continue;
            
            var next = handlerDelegate;
            handlerDelegate = ct =>
                ((dynamic)behavior).HandleAsync((dynamic)request, next, ct);
        }

        return await handlerDelegate(cancellationToken);
    }

    public async Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken)
        where TNotification : INotification
    {
        var handlers = _provider.GetServices<INotificationHandler<TNotification>>();
        var tasks = handlers.Select(h => h.HandleAsync(notification, cancellationToken));
        await Task.WhenAll(tasks);
    }
}
