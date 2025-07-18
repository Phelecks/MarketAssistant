using BlockProcessor.Application.WalletAddress.Commands.DeleteWalletAddress;
using MassTransit;
using MassTransitManager.Events.Interfaces;
using MediatR.Interfaces;

namespace BlockProcessor.Infrastructure.MassTransit.Consumers.Events;

public class DeleteWalletAddressEventConsumer(IRequestDispatcher dispatcher) : IConsumer<IDeleteBlockProcessingWalletAddressEvent>
{
    private readonly IRequestDispatcher _dispatcher = dispatcher;

    public async Task Consume(ConsumeContext<IDeleteBlockProcessingWalletAddressEvent> context)
    {
        await _dispatcher.SendAsync(new DeleteWalletAddressCommand(context.Message.WalletAddress), context.CancellationToken);
    }
}