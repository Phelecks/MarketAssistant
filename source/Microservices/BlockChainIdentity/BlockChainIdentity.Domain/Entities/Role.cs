using BaseDomain.Common;
using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Domain.Entities;

public class Role : BaseAuditEntity
{
    [Required]
    public required string Title { get; set; }

    public virtual ICollection<WalletRole> WalletRoles { get; set; }
}