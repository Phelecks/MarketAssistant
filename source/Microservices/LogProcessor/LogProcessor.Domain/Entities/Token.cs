using BaseDomain.Common;
using System.ComponentModel.DataAnnotations;

namespace LogProcessor.Domain.Entities;
public class Token : BaseEntity
{
    [Required]
    public Nethereum.Signer.Chain Chain { get; set; }

    /// <summary>
    /// For not-main tokens like USDC or USDT and smart contracts
    /// </summary>
    [Required]
    public required string ContractAddress { get; set; }

    public string? StakeContractAddress { get; set; }

    public string? OwnerWalletAddress { get; set; }

    public string? RoyaltyWalletAddress { get; set; }

    public int Decimals { get; set; }
}
