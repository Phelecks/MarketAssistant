using System.Collections.Concurrent;
using WalletTracker.Application.Interfaces;

namespace WalletTracker.Infrastructure.Services;

public class AddressService : IAddressService
{
    private ConcurrentBag<Domain.Entities.DestinationAddress> _addresses = new ConcurrentBag<Domain.Entities.DestinationAddress>();

    public void SetDestinationAddress(Domain.Entities.DestinationAddress address)
    {
        _addresses.Add(address);
    }

    public Domain.Entities.DestinationAddress? GetDestinationAddress(Nethereum.Signer.Chain chain)
    {
        return _addresses.FirstOrDefault(x => x.chain == chain);
    }
}
