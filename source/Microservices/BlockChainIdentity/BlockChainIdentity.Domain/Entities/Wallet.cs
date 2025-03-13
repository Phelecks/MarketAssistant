using BaseDomain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlockChainIdentity.Domain.Entities;

public class Wallet : BaseAuditEntityWithNoPrimaryKey
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required string Address { get; set; }

    public int ChainId { get; set; }

    public virtual ICollection<WalletRole> WalletRoles { get; set; }
    public virtual ICollection<Token> Tokens { get; set; }
}