using LogProcessor.Application.Token.Commands.DeleteToken;
using MassTransit;
using MassTransitManager.Events.Interfaces;
using MediatR.Interfaces;

namespace LogProcessor.Infrastructure.MassTransit.Consumers.Events;

public class DeleteTokenEventConsumer(IRequestDispatcher dispatcher) : IConsumer<IDeleteLogProcessingTokenEvent>
{
    private readonly IRequestDispatcher _dispatcher = dispatcher;

    public async Task Consume(ConsumeContext<IDeleteLogProcessingTokenEvent> context)
    {
        await _dispatcher.SendAsync(new DeleteTokenCommand(context.Message.ContractAddress, context.Message.Chain), context.CancellationToken);
    }
}