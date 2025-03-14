using System.ComponentModel.DataAnnotations;
using BaseDomain.Common;

namespace BlockChainIdentity.Domain.Entities;

public class RpcUrl : BaseEntity
{
    private int _chain;
    [Required]
    public Nethereum.Signer.Chain Chain
    {
        get => (Nethereum.Signer.Chain)_chain;
        set => _chain = (int)value;
    }

    private string _url = string.Empty;
    [Required]
    public Uri Uri
    {
        get => new(_url);
        set => _url = value.ToString();
    }
}