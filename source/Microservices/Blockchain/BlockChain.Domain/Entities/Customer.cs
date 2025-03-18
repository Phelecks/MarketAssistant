using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BaseDomain.Common;

namespace BlockChain.Domain.Entities;

public class Customer : BaseAuditEntityWithNoPrimaryKey
{
    [Required]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required string WalletAddress { get; set; }

    public virtual ICollection<Track>? Tracks { get; set; }
}
