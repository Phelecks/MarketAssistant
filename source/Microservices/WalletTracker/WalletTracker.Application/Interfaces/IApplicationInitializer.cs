namespace WalletTracker.Application.Interfaces;

public interface IApplicationInitializer
{
    void Initialize(List<Domain.Entities.Token> tokens, List<Domain.Entities.RpcUrl> rpcUrls, List<Domain.Entities.DestinationAddress> destinationAddresses);
}
