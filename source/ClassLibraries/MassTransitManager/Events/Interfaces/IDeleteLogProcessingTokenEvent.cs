using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface IDeleteLogProcessingTokenEvent : CorrelatedBy<Guid>
{
    Nethereum.Signer.Chain Chain { get; }
    string ContractAddress { get; }
}