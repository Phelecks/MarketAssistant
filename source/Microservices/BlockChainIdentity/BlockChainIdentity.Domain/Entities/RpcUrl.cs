using System.ComponentModel.DataAnnotations;
using BaseDomain.Common;

namespace BlockChainIdentity.Domain.Entities;

public class RpcUrl : BaseEntity
{
    [Required]
    public Nethereum.Signer.Chain Chain { get; set; }

    private string _url = string.Empty;
    [Required]
    public Uri Uri
    {
        get => new(_url);
        set => _url = value.ToString();
    }
}