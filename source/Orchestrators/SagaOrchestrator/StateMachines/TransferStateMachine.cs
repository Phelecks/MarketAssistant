using MassTransit;
using MassTransitManager.Helpers;
using MassTransitManager.Messages;
using MassTransitManager.Messages.Interfaces;
using SagaOrchestrator.StatemachineInstances;

namespace SagaOrchestrator.StateMachines;

public class TransferStateMachine : MassTransitStateMachine<TransferStateMachineInstance>
{
    #region States
    public State TransferConfirmed { get; private set; } = null!;
    public State Accepted { get; private set; } = null!;
    #endregion

    #region Events
    public Event<ITransferConfirmedMessage> TransferConfirmedEvent { get; private set; } = null!;
    #endregion

    public TransferStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => TransferConfirmedEvent, x => 
        {
            x.CorrelateById(context => context.Message.CorrelationId);
        });

        Initially(
            When(TransferConfirmedEvent)
                .Then(context =>
                {
                    context.Saga.Chain = context.Message.Chain;
                    context.Saga.Hash = context.Message.Hash;
                    context.Saga.From = context.Message.From;
                    context.Saga.To = context.Message.To;
                    context.Saga.Value = context.Message.Value;
                    context.Saga.DateTime = context.Message.DateTime;
                    context.Saga.Erc20Transfers = context.Message.Erc20Transfers;
                    context.Saga.Erc721Transfers = context.Message.Erc721Transfers;
                })
                .Send(
                    destinationAddress: new Uri($"queue:{Queues.NotifyTransferConfirmedMessageQueueName}"),
                    context => new NotifyTransferConfirmedMessage(
                        correlationId: context.Saga.CorrelationId, 
                        userId: "admin", 
                        discord: null,
                        transfer: new NotifyTransferConfirmedMessage.Transfer(
                            Chain: context.Saga.Chain,
                            Hash: context.Saga.Hash,
                            From: context.Saga.From,
                            To: context.Saga.To,
                            Value: context.Saga.Value,
                            DateTime: context.Saga.DateTime, 
                            Erc20Transfers: context.Saga.Erc20Transfers, 
                            Erc721Transfers: context.Saga.Erc721Transfers)))
                .TransitionTo(TransferConfirmed)
                .Finalize()
            );
    }
}