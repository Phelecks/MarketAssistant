using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class CreateLogProcessingTokenEvent(Guid correlationId, Nethereum.Signer.Chain chain, string contractAddress,
    string? stakeContractAddress, string? ownerWalletAddress, string? royaltyWalletAddress, int decimals) : ICreateLogProcessingTokenEvent
{
    public Guid CorrelationId { get; } = correlationId;
    public Nethereum.Signer.Chain Chain { get; } = chain;
    public string ContractAddress { get; } = contractAddress;
    public string? StakeContractAddress { get; } = stakeContractAddress;
    public string? OwnerWalletAddress { get; } = ownerWalletAddress;
    public string? RoyaltyWalletAddress { get; } = royaltyWalletAddress;
    public int Decimals { get; } = decimals;
}