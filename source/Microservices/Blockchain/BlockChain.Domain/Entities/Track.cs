using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BaseDomain.Common;

namespace BlockChain.Domain.Entities;

public class Track : BaseAuditEntity
{
    [Required]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required string UserId { get; set; }

    [Required]
    public required string WalletAddress { get; set; }

    [ForeignKey("Customer")]
    public required string CustomerWalletAddress { get; set; }
    public virtual Customer Customer { get; set; }
}
