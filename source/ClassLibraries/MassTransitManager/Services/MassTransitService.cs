using MassTransit;

namespace MassTransitManager.Services;

public class MassTransitService : IMassTransitService
{
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitService(ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
    {
        _sendEndpointProvider = sendEndpointProvider;
        _publishEndpoint = publishEndpoint;
    }

    /// <summary>
    /// When a message is sent, it is delivered to a specific endpoint using a DestinationAddress.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="message"></param>
    /// <param name="queueName"></param>
    /// <returns></returns>
    public async Task SendAsync<T>(T message, string queueName, CancellationToken cancellationToken = default) where T : class
    {
        var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queueName}"));

        await sendEndpoint.Send<T>(message, cancellationToken);
    }

    /// <summary>
    /// When a message is published, it is not sent to a specific endpoint, but is instead broadcasted to any consumers which have subscribed to the message type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        await _publishEndpoint.Publish<T>(message, cancellationToken);
    }
}