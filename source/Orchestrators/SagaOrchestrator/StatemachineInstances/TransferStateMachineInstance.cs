using MassTransit;
using MassTransitManager.Events.Interfaces;

namespace SagaOrchestrator.StatemachineInstances;

public class TransferStateMachineInstance : SagaStateMachineInstance, ITransferConfirmedEvent
{
    public Guid CorrelationId { get; set; }

    public required State CurrentState { get; set; }

    public Nethereum.Signer.Chain Chain { get; set; }

    public required string Hash { get; set; }

    public required string From { get; set; }

    public required string To { get; set; }

    public decimal Value { get; set; }

    public DateTime DateTime { get; set; }

    public List<ITransferConfirmedEvent.Erc20Transfer>? Erc20Transfers { get; set; }

    public List<ITransferConfirmedEvent.Erc721Transfer>? Erc721Transfers { get; set; }
}