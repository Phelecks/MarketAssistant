using BaseDomain.Common;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Domain.Entities;

public class Role : BaseAuditEntity
{
    [Required]
    public string title { get; set; }

    public virtual ICollection<WalletRole> walletRoles { get; set; }
}