using BaseDomain.Common;
using System.ComponentModel.DataAnnotations;

namespace WalletTracker.Domain.Entities;

public class DestinationAddress : BaseEntity
{
    public DestinationAddress(long id, Nethereum.Signer.Chain chain, string address)
    {
        Id = id;
        _chain = (int)chain;
        Address = address;
    }

    private int _chain;
    [Required]
    public Nethereum.Signer.Chain chain
    {
        get => (Nethereum.Signer.Chain)_chain;
        set => _chain = (int)value;
    }

    public string Address { get; set; }
}
