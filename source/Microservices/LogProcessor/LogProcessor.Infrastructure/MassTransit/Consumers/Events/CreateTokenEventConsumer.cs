using LogProcessor.Application.Token.Commands.CreateToken;
using MassTransit;
using MassTransitManager.Events.Interfaces;
using MediatR.Interfaces;

namespace LogProcessor.Infrastructure.MassTransit.Consumers.Events;

public class CreateTokenEventConsumer(IRequestDispatcher dispatcher) : IConsumer<ICreateLogProcessingTokenEvent>
{
    private readonly IRequestDispatcher _dispatcher = dispatcher;

    public async Task Consume(ConsumeContext<ICreateLogProcessingTokenEvent> context)
    {
       await _dispatcher.SendAsync(new CreateTokenCommand(context.Message.Chain, context.Message.ContractAddress, 
            context.Message.StakeContractAddress, context.Message.OwnerWalletAddress, context.Message.RoyaltyWalletAddress,
            context.Message.Decimals), context.CancellationToken);
    }
}