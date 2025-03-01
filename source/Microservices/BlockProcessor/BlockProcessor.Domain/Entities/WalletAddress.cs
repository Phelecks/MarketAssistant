using System.ComponentModel.DataAnnotations;
using BaseDomain.Common;

namespace BlockProcessor.Domain.Entities;

public class WalletAddress : BaseAuditEntity
{
    [Required]
    public required string Address { get; set; }
}