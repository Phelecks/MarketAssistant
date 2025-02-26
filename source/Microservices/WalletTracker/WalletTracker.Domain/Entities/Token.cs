using BaseDomain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WalletTracker.Domain.Entities;
public class Token : BaseEntityWithNoPrimaryKey
{
    public Token(long id, string symbol, Nethereum.Signer.Chain chain, BaseDomain.Enums.BlockChainEnums.TokenType tokenType, bool enabled, string? contractAddress, int decimals)
    {
        this.id = id;
        this.symbol = symbol;
        _chain = (int)chain;
        this.tokenType = tokenType;
        this.enabled = enabled;
        this.contractAddress = contractAddress;
        this.decimals = decimals;
    }

    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long id { get; set; }

    [Required]
    public string symbol { get; set; }

    private int _chain;
    [Required]
    public Nethereum.Signer.Chain chain {
        get => (Nethereum.Signer.Chain)_chain;
        set => _chain = (int)value;
    }

    public BaseDomain.Enums.BlockChainEnums.TokenType tokenType { get; set; }

    public bool enabled { get; set; }

    public string? contractAddress { get; set; }

    public int decimals { get; set; }
}