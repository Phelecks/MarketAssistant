using BaseDomain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WalletTracker.Domain.Entities;
public class Token : BaseEntityWithNoPrimaryKey
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long id { get; set; }

    [Required]
    public string symbol { get; set; }

    private int _chain;
    [Required]
    public Nethereum.Signer.Chain chain {
        get => (Nethereum.Signer.Chain)_chain;
        set => _chain = (int)chain;
    }

    public BaseDomain.Enums.BlockChainEnums.TokenType tokenType { get; set; }

    public bool enabled { get; set; }

    public string? contractAddress { get; set; }

    public int decimals { get; set; }
}