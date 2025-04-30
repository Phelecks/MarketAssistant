using LogProcessor.Application.WalletAddress.Commands.CreateWalletAddress;
using MassTransit;
using MassTransitManager.Events.Interfaces;
using MediatR;

namespace LogProcessor.Infrastructure.MassTransit.Consumers.Events;

public class CreateTokenEventConsumer(ISender sender) : IConsumer<ICreateLogProcessingTokenEvent>
{
    private readonly ISender _sender = sender;

    public async Task Consume(ConsumeContext<ICreateLogProcessingTokenEvent> context)
    {
       await _sender.Send(new CreateTokenCommand(context.Message.Chain, context.Message.ContractAddress, 
            context.Message.StakeContractAddress, context.Message.OwnerWalletAddress, context.Message.RoyaltyWalletAddress,
            context.Message.Decimals), context.CancellationToken);
    }
}