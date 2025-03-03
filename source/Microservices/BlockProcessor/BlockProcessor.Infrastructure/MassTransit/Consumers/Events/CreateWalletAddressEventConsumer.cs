using BlockProcessor.Application.WalletAddress.Commands.CreateWalletAddress;
using MassTransit;
using MassTransitManager.Events.Interfaces;
using MediatR;

namespace BlockProcessor.Infrastructure.MassTransit.Consumers.Events;

public class CreateWalletAddressEventConsumer(ISender sender) : IConsumer<ICreateBlockProcessingWalletAddressEvent>
{
    private readonly ISender _sender = sender;

    public async Task Consume(ConsumeContext<ICreateBlockProcessingWalletAddressEvent> context)
    {
       await _sender.Send(new CreateWalletAddressCommand(context.Message.WalletAddress), context.CancellationToken);
    }
}