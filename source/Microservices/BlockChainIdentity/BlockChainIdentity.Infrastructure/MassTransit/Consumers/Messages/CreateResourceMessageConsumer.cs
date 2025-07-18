using BlockChainIdentity.Application.Resource.Commands.CreateResource;
using LoggerService.Helpers;
using MassTransit;
using MassTransitManager.Messages.Interfaces;
using MediatR.Interfaces;
using Microsoft.Extensions.Logging;

namespace BlockChainIdentity.Infrastructure.MassTransit.Consumers.Messages;

public class CreateResourceMessageConsumer : IConsumer<ICreateResourceMessage>
{
    private readonly IRequestDispatcher _dispatcher;
    private readonly ILogger<CreateResourceMessageConsumer> _logger;

    public CreateResourceMessageConsumer(IRequestDispatcher dispatcher, ILogger<CreateResourceMessageConsumer> logger)
    {
        _dispatcher = dispatcher;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<ICreateResourceMessage> context)
    {
        try
        {
            await _dispatcher.SendAsync(new CreateResourceCommand(context.Message.Title), context.CancellationToken);
        }
        catch (Exception exception)
        {
            _ = Task.Run(() => _logger.LogError(
                       eventId: EventTool.GetEventInformation(eventType: EventType.Exception, eventName: "Create Resource Error"),
                       exception: exception, exception.Message), context.CancellationToken);
            await Task.CompletedTask;
        }
    }
}