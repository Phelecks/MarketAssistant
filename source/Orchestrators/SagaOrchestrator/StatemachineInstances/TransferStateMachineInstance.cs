using MassTransit;
using MassTransitManager.Messages.Interfaces;

namespace SagaOrchestrator.StatemachineInstances;

public class TransferStateMachineInstance : SagaStateMachineInstance, ITransferConfirmedMessage
{
    public Guid CorrelationId { get; set; }

    public required State CurrentState { get; set; }

    public Nethereum.Signer.Chain Chain { get; set; }

    public required string Hash { get; set; }

    public required string From { get; set; }

    public required string To { get; set; }

    public decimal Value { get; set; }

    public DateTime DateTime { get; set; }

    public List<ITransferConfirmedMessage.Erc20Transfer>? Erc20Transfers { get; set; }

    public List<ITransferConfirmedMessage.Erc721Transfer>? Erc721Transfers { get; set; }
}