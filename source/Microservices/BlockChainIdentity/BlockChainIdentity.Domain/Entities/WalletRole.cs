using BaseDomain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlockChainIdentity.Domain.Entities;

public class WalletRole : BaseAuditEntity
{
    [ForeignKey("wallet")]
    public string walletAddress { get; set; }
    public virtual Wallet wallet { get; set; }

    [ForeignKey("role")]
    public long roleId { get; set; }
    public virtual Role role { get; set; }
}