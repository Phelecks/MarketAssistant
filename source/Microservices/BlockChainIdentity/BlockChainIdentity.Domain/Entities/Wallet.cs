using BaseDomain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlockChainIdentity.Domain.Entities;

public class Wallet : BaseAuditEntityWithNoPrimaryKey
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string address { get; set; }

    public int chainId { get; set; }

    public virtual ICollection<WalletRole> walletRoles { get; set; }
    public virtual ICollection<Token> tokens { get; set; }
}