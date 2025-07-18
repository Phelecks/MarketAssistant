using MediatR.Attributes;
using MediatR.Interfaces;
using FluentValidation;
using LoggerService.Helpers;
using Microsoft.Extensions.Logging;

namespace MediatR.Behaviors;

[BehaviorOrder(2)]
public class FluentValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators, ILogger<FluentValidationBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;
    private readonly ILogger<FluentValidationBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> HandleAsync(TRequest request, Func<CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            failures.ForEach(failure =>
                {
                    _ = Task.Run(() => _logger.LogWarning(
                        eventId: EventTool.GetEventInformation(eventType: EventType.Warning, eventName: "Validation Error"),
                        message: "Validation error, name: {@requestName}, request: {@request}, errorMessage: {@errorMessage}",
                        typeof(TRequest).Name, request, failure.ErrorMessage), cancellationToken);
                }
            );
            throw new ValidationException(failures);
        }
        return await next(cancellationToken);
    }
}
