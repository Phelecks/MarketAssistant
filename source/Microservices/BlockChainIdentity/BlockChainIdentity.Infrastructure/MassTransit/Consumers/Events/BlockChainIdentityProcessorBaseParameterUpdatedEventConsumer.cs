using BlockChainIdentity.Application.BaseParameter.Commands.CreateBaseParameter;
using MassTransit;
using MassTransitManager.Events;
using MediatR;

namespace BlockChainIdentity.Infrastructure.MassTransit.Consumers.Events;

public class BlockChainIdentityProcessorBaseParameterUpdatedEventConsumer : IConsumer<BlockChainIdentityBaseParameterUpdatedEvent>
{
    private readonly ISender _sender;

    public BlockChainIdentityProcessorBaseParameterUpdatedEventConsumer(ISender sender)
    {
        _sender = sender;
    }
    public async Task Consume(ConsumeContext<BlockChainIdentityBaseParameterUpdatedEvent> context)
    {
        try
        {
            await _sender.Send(new CreateBaseParameterCommand(context.Message.Category, context.Message.Field, context.Message.Value, context.Message.KernelBaseParameterId), context.CancellationToken);
        }
        catch (Exception)
        {
            await Task.CompletedTask;
        }
    }
}