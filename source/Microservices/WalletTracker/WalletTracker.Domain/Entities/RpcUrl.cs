using BaseDomain.Common;
using System.ComponentModel.DataAnnotations;

namespace WalletTracker.Domain.Entities;

public class RpcUrl : BaseEntity
{
    private int _chain;
    [Required]
    public Nethereum.Signer.Chain chain
    {
        get => (Nethereum.Signer.Chain)_chain;
        set => _chain = (int)chain;
    }

    public string rpcUrl { get; set; }
}
