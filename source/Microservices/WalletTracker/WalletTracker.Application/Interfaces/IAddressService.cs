namespace WalletTracker.Application.Interfaces;

public interface IAddressService
{
    void SetDestinationAddress(Domain.Entities.DestinationAddress address);
    Domain.Entities.DestinationAddress? GetDestinationAddress(Nethereum.Signer.Chain chain);
}
