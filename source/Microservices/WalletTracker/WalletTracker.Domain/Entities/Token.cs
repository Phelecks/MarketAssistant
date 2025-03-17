using BaseDomain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WalletTracker.Domain.Entities;
public class Token(long id, string symbol, Nethereum.Signer.Chain chain, BaseDomain.Enums.BlockChainEnums.TokenType tokenType, bool enabled, string? contractAddress, int decimals) : BaseEntityWithNoPrimaryKey
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; set; } = id;

    [Required]
    public string Symbol { get; set; } = symbol;

    [Required]
    public Nethereum.Signer.Chain Chain { get; set; } = chain;

    public BaseDomain.Enums.BlockChainEnums.TokenType TokenType { get; set; } = tokenType;

    public bool Enabled { get; set; } = enabled;

    public string? ContractAddress { get; set; } = contractAddress;

    public int Decimals { get; set; } = decimals;
}