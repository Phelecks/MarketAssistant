using Informing.Application.BaseParameter.Commands.CreateBaseParameter;
using MassTransit;
using MassTransitManager.Events;
using MediatR;

namespace Informing.Infrastructure.MassTransit.Consumers.Events;

public class InformingBaseParameterUpdatedEventConsumer : IConsumer<InformingBaseParameterUpdatedEvent>
{
    private readonly ISender _sender;

    public InformingBaseParameterUpdatedEventConsumer(ISender sender)
    {
        _sender = sender;
    }
    public async Task Consume(ConsumeContext<InformingBaseParameterUpdatedEvent> context)
    {
        try
        {
            await _sender.Send(new CreateBaseParameterCommand(context.Message.Category, context.Message.Field, context.Message.Value, context.Message.KernelBaseParameterId));
        }
        catch (Exception)
        {
        }
    }
}