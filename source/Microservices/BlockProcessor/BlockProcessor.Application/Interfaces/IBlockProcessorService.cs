namespace BlockProcessor.Application.Interfaces;

public interface IBlockProcessorService
{
    Task StartAsync(Nethereum.Signer.Chain chain, CancellationToken cancellationToken);
    Task StopAsync(Nethereum.Signer.Chain chain,CancellationToken cancellationToken);
    Task RestartAsync(Nethereum.Signer.Chain chain, CancellationToken cancellationToken);
    void SyncWalletAddresses(List<string> walletAddress);
    void PurgeWalletAddresses();
}