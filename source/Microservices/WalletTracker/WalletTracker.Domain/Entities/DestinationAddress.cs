using BaseDomain.Common;
using System.ComponentModel.DataAnnotations;

namespace WalletTracker.Domain.Entities;

public class DestinationAddress : BaseEntity
{
    public DestinationAddress(long id, Nethereum.Signer.Chain chain, string address)
    {
        Id = id;
        Chain = chain;
        Address = address;
    } 
    [Required]
    public Nethereum.Signer.Chain Chain { get; set; }

    public string Address { get; set; }
}
