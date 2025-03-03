using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BaseDomain.Common;

namespace BlockProcessor.Domain.Entities;

public class WalletAddress : BaseAuditEntityWithNoPrimaryKey
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required string Address { get; set; }
}