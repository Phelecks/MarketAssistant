using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BaseDomain.Common;

namespace BlockChain.Domain.Entities;

public class Notification : BaseAuditEntity
{
    public NotificationType Type { get; set; }

    [Required]
    public required string Identifier { get; set; }

    [ForeignKey("Customer")]
    public required string CustomerWalletAddress { get; set; }
    public virtual Customer Customer { get; set; }


    public enum NotificationType { Discord }
}
