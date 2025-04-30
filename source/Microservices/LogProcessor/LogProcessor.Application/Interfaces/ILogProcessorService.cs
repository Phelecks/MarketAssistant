namespace LogProcessor.Application.Interfaces;

public interface ILogProcessorService
{
    Task StartAsync(Nethereum.Signer.Chain chain, CancellationToken cancellationToken);
    Task StopAsync(Nethereum.Signer.Chain chain,CancellationToken cancellationToken);
    Task RestartAsync(Nethereum.Signer.Chain chain, CancellationToken cancellationToken);
}