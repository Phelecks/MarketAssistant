using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BaseDomain.Common;

namespace BlockProcessor.Domain.Entities;

public class Erc721Transfer : BaseEntity
{
    [Required]
    public required string From { get; set; }

    [Required]
    public required string To { get; set; }

    [Required]
    public required string ContractAddress { get; set; }

    [Required]
    public long TokenId { get; set; }

    [ForeignKey("Transfer")]
    public Guid TransferId { get; set; }
    public virtual Transfer Transfer { get; set; }
}