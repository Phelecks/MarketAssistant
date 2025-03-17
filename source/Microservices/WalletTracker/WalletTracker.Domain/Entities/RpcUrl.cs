using BaseDomain.Common;
using System.ComponentModel.DataAnnotations;

namespace WalletTracker.Domain.Entities;

public class RpcUrl : BaseEntity
{
    public RpcUrl(long id, Nethereum.Signer.Chain chain, Uri uri)
    {
        Id = id;
        Chain = chain;
        Uri = uri;
    }

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
