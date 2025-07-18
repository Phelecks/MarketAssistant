using BlockProcessor.Application.WalletAddress.Commands.CreateWalletAddress;
using MassTransit;
using MassTransitManager.Events.Interfaces;
using MediatR.Interfaces;

namespace BlockProcessor.Infrastructure.MassTransit.Consumers.Events;

public class CreateWalletAddressEventConsumer(IRequestDispatcher dispatcher) : IConsumer<ICreateBlockProcessingWalletAddressEvent>
{
    private readonly IRequestDispatcher _dispatcher = dispatcher;

    public async Task Consume(ConsumeContext<ICreateBlockProcessingWalletAddressEvent> context)
    {
       await _dispatcher.SendAsync(new CreateWalletAddressCommand(context.Message.WalletAddress), context.CancellationToken);
    }
}