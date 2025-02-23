namespace WalletTracker.Application.Interfaces;

public interface IRpcUrlService
{
    void AddRpcUrl(Domain.Entities.RpcUrl rpcUrl);
    string GetRpcUrl(Nethereum.Signer.Chain chain);
}
