using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BaseDomain.Helpers;
using BaseDomain.Common;

namespace BlockProcessor.Domain.Entities;

public class Transfer : BaseAuditEntityWithNoPrimaryKey
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    
    [Required]
    public required string Hash { get; set; }

    [Required]
    public required string From { get; set; }

    [Required]
    public required string To { get; set; }

    private int _chain;
    [Required]
    public Nethereum.Signer.Chain Chain
    {
        get => (Nethereum.Signer.Chain)_chain;
        set => _chain = (int)value;
    }

    private decimal _value;
    [Required]
    [Range(minimum: 0, maximum: double.MaxValue)]
    [Column(TypeName = "decimal(38,19)")]
    public decimal Value
    {
        get => _value.Normalize();
        set => _value = value.Normalize();
    }

    private decimal _gasUsed;
    [Required]
    [Range(minimum: 0, maximum: double.MaxValue)]
    [Column(TypeName = "decimal(38,19)")]
    public decimal GasUsed
    {
        get => _gasUsed.Normalize();
        set => _gasUsed = value.Normalize();
    }

    private decimal _effectiveGasPrice;
    [Required]
    [Range(minimum: 0, maximum: double.MaxValue)]
    [Column(TypeName = "decimal(38,19)")]
    public decimal EffectiveGasPrice
    {
        get => _effectiveGasPrice.Normalize();
        set => _effectiveGasPrice = value.Normalize();
    }

    private decimal _cumulativeGasUsed;
    [Required]
    [Range(minimum: 0, maximum: double.MaxValue)]
    [Column(TypeName = "decimal(38,19)")]
    public decimal CumulativeGasUsed
    {
        get => _cumulativeGasUsed.Normalize();
        set => _cumulativeGasUsed = value.Normalize();
    }

    [Required]
    public long BlockNumber { get; set; }

    [Required]
    public DateTime ConfirmedDatetime { get; set; }

    public virtual ICollection<Erc20Transfer>? Erc20Transfers { get; set; }
}