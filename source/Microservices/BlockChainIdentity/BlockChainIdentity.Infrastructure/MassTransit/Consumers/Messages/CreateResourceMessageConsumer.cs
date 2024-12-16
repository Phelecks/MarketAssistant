using BlockChainIdentity.Application.Resource.Commands.CreateResource;
using LoggerService.Helpers;
using MassTransit;
using MassTransitManager.Messages.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BlockChainIdentity.Infrastructure.MassTransit.Consumers.Messages;

public class CreateResourceMessageConsumer : IConsumer<ICreateResourceMessage>
{
    private readonly ISender _sender;
    private readonly ILogger<CreateResourceMessageConsumer> _logger;

    public CreateResourceMessageConsumer(ISender sender, ILogger<CreateResourceMessageConsumer> logger)
    {
        _sender = sender;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<ICreateResourceMessage> context)
    {
        try
        {
            await _sender.Send(new CreateResourceCommand(context.Message.Title));
        }
        catch (Exception exception)
        {
            _ = Task.Run(() => _logger.LogError(
                       eventId: EventTool.GetEventInformation(eventType: EventType.IdentityException, eventName: "Create Resource Error"),
                       exception: exception, exception.Message), context.CancellationToken);
            await Task.CompletedTask;
        }
    }
}