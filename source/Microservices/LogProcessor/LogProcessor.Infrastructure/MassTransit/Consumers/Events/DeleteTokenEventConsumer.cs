using LogProcessor.Application.Token.Commands.DeleteToken;
using MassTransit;
using MassTransitManager.Events.Interfaces;
using MediatR;

namespace LogProcessor.Infrastructure.MassTransit.Consumers.Events;

public class DeleteTokenEventConsumer(ISender sender) : IConsumer<IDeleteLogProcessingTokenEvent>
{
    private readonly ISender _sender = sender;

    public async Task Consume(ConsumeContext<IDeleteLogProcessingTokenEvent> context)
    {
        await _sender.Send(new DeleteTokenCommand(context.Message.ContractAddress, context.Message.Chain), context.CancellationToken);
    }
}