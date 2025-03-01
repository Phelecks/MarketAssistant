using System.ComponentModel.DataAnnotations;
using BaseDomain.Common;

namespace BlockProcessor.Domain.Entities;

public class RpcUrl : BaseEntity
{
    private int _chain;
    [Required]
    public Nethereum.Signer.Chain chain
    {
        get => (Nethereum.Signer.Chain)_chain;
        set => _chain = (int)value;
    }

    [Required]
    public required string rpcUrl { get; set; }
}