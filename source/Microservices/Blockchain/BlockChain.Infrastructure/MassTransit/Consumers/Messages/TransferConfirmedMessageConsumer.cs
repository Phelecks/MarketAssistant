using BlockChain.Application.Customer.Commands.ProcessTransferConfirmed;
using MassTransit;
using MassTransitManager.Messages;
using MassTransitManager.Messages.Interfaces;
using MediatR.Interfaces;
using MediatR.Interfaces;

namespace BlockChain.Infrastructure.MassTransit.Consumers.Messages;

public class TransferConfirmedMessageConsumer(IRequestDispatcher dispatcher) : IConsumer<ITransferConfirmedMessage>
{
    private readonly IRequestDispatcher _dispatcher = dispatcher;

    public async Task Consume(ConsumeContext<ITransferConfirmedMessage> context)
    {
        await _dispatcher.SendAsync(new ProcessTransferConfirmedCommand(
            TransferConfirmedEvent: new TransferConfirmedMessage(
                correlationId: context.Message.CorrelationId,
                transfer: new TransferConfirmedMessage.Transfer(
                    Chain: context.Message.Chain,
                    Hash: context.Message.Hash,
                    From: context.Message.From,
                    To: context.Message.To,
                    Value: context.Message.Value,
                    DateTime: context.Message.DateTime,
                    Erc20Transfers: context.Message.Erc20Transfers,
                    Erc721Transfers: context.Message.Erc721Transfers
                ))), context.CancellationToken);

    }
}