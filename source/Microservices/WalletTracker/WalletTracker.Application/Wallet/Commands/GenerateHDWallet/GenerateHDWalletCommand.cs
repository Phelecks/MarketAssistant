using BaseApplication.Interfaces;
using BlockChainHDWalletHelper.Interfaces;
using ExecutorManager.Helpers;
using MediatR.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace WalletTracker.Application.Wallet.Commands.GenerateHDWallet;


public record GenerateHDWalletCommand(NBitcoin.WordCount WordCount) : IRequest<Nethereum.HdWallet.Wallet>;

public class CreateContactCommandHandler : IRequestHandler<GenerateHDWalletCommand, Nethereum.HdWallet.Wallet>
{
    private readonly IHdWalletService _hdWalletService;
    private readonly ICypherService _cypherService;
    private readonly ResiliencePipeline _pollyPipeline;

    public CreateContactCommandHandler(IHdWalletService hdWalletService,
        ICypherService cypherService,
        [FromKeyedServices(PipelineHelper.RetryEverythingFiveTimes)] ResiliencePipeline pollyPipeline)
    {
        _hdWalletService = hdWalletService;
        _pollyPipeline = pollyPipeline;
        _cypherService = cypherService;
    }

    public async Task<Nethereum.HdWallet.Wallet> HandleAsync(GenerateHDWalletCommand request, CancellationToken cancellationToken)
    {
        var seedPassword = _cypherService.GeneratePassword(16);
        var hdWallet = _pollyPipeline.Execute(() => _hdWalletService.GenerateWallet(seedPassword, request.WordCount));
        return await Task.FromResult(hdWallet);
    }
}
