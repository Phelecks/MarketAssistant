using BaseDomain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlockChainIdentity.Domain.Entities;

public class WalletRole : BaseAuditEntity
{
    [ForeignKey("wallet")]
    public required string WalletAddress { get; set; }
    public virtual Wallet Wallet { get; set; }

    [ForeignKey("role")]
    public long RoleId { get; set; }
    public virtual Role Role { get; set; }
}