namespace WalletTracker.Application.Interfaces;

public interface IRpcUrlService
{
    void AddRpcUrl(Nethereum.Signer.Chain chain, string rpcUrl);
    string? GetRpcUrl(Nethereum.Signer.Chain chain);
}
