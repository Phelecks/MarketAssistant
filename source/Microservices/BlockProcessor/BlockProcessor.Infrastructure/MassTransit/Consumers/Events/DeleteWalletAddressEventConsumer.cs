using BlockProcessor.Application.WalletAddress.Commands.DeleteWalletAddress;
using MassTransit;
using MassTransitManager.Events.Interfaces;
using MediatR;

namespace BlockProcessor.Infrastructure.MassTransit.Consumers.Events;

public class DeleteWalletAddressEventConsumer(ISender sender) : IConsumer<IDeleteBlockProcessingWalletAddressEvent>
{
    private readonly ISender _sender = sender;

    public async Task Consume(ConsumeContext<IDeleteBlockProcessingWalletAddressEvent> context)
    {
        await _sender.Send(new DeleteWalletAddressCommand(context.Message.WalletAddress), context.CancellationToken);
    }
}