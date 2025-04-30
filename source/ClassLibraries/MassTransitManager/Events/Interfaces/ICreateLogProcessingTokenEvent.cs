using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ICreateLogProcessingTokenEvent : CorrelatedBy<Guid>
{
    Nethereum.Signer.Chain Chain { get; }
    string ContractAddress { get; }
    string? StakeContractAddress { get; }
    string? OwnerWalletAddress { get; }
    string? RoyaltyWalletAddress { get; }
    int Decimals { get; }
}