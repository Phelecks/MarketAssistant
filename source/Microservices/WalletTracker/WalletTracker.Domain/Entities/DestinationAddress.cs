using BaseDomain.Common;
using System.ComponentModel.DataAnnotations;

namespace WalletTracker.Domain.Entities;

public class DestinationAddress : BaseEntity
{
    public DestinationAddress(long id, Nethereum.Signer.Chain chain, string address)
    {
        this.id = id;
        _chain = (int)chain;
        this.address = address;
    }

    private int _chain;
    [Required]
    public Nethereum.Signer.Chain chain
    {
        get => (Nethereum.Signer.Chain)_chain;
        set => _chain = (int)value;
    }

    public string address { get; set; }
}
