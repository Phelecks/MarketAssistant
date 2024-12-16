namespace MassTransitManager.Services;

public interface IMassTransitService
{
    /// <summary>
    /// When a message is sent, it is delivered to a specific endpoint using a DestinationAddress.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="message"></param>
    /// <param name="queueName"></param>
    /// <returns></returns>
    Task SendAsync<T>(T message, string queueName, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// When a message is published, it is not sent to a specific endpoint, but is instead broadcasted to any consumers which have subscribed to the message type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="message"></param>
    /// <returns></returns>
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;
}