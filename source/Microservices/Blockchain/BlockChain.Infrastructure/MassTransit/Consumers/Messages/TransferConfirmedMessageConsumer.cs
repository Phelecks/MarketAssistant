using BlockChain.Application.Customer.Commands.ProcessTransferConfirmed;
using MassTransit;
using MassTransitManager.Messages;
using MassTransitManager.Messages.Interfaces;
using MediatR;

namespace BlockChain.Infrastructure.MassTransit.Consumers.Messages;

public class TransferConfirmedMessageConsumer(ISender sender) : IConsumer<ITransferConfirmedMessage>
{
    private readonly ISender _sender = sender;

    public async Task Consume(ConsumeContext<ITransferConfirmedMessage> context)
    {
        await _sender.Send(new ProcessTransferConfirmedCommand(
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
                ))));

    }
}