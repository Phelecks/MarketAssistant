using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BaseDomain.Helpers;
using BaseDomain.Common;

namespace BlockProcessor.Domain.Entities;

public class Erc20Transfer : BaseEntity
{
    [Required]
    public required string From { get; set; }

    [Required]
    public required string To { get; set; }

    private decimal _value;
    [Required]
    [Range(minimum: 0, maximum: double.MaxValue)]
    [Column(TypeName = "decimal(38,19)")]
    public decimal Value
    {
        get => _value.Normalize();
        set => _value = value.Normalize();
    }

    public required string ContractAddress { get; set; }

    [ForeignKey("Transfer")]
    public Guid TransferId { get; set; }
    public virtual Transfer Transfer { get; set; }
}