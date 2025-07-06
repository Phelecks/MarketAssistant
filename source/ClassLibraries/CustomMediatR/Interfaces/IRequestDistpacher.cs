namespace CustomMediatR.Interfaces;

public interface IRequestDistpacher
{
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken);
    Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken)
        where TNotification : INotification;   
}