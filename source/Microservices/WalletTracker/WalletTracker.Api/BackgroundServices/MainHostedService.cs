using BlockChainHDWalletHelper.Interfaces;
using LoggerService.Helpers;
using MediatR;
using WalletTracker.Application.Interfaces;
using WalletTracker.Application.Track.Commands.TrackWallet;
using WalletTracker.Application.Wallet.Commands.GenerateHDWallet;

namespace WalletTracker.Api.BackgroundServices;

public class MainHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MainHostedService> _logger;
    private readonly IConfiguration _configuration;

    public MainHostedService(IServiceProvider serviceProvider, ILogger<MainHostedService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        Initialize();

        while (!cancellationToken.IsCancellationRequested)
        {
            var intervalsInMinutes = 1;
            _ = Task.Run(() => _logger.LogInformation(
                 eventId: EventTool.GetEventInformation(eventType: EventType.GameBackgroundTasks, eventName: $"{nameof(MainHostedService)}"),
                 "{@hostedServiceName} is delaying for {@delay} minutes.", nameof(MainHostedService), intervalsInMinutes), cancellationToken);
            await Task.Delay(TimeSpan.FromMinutes(intervalsInMinutes), cancellationToken);
        }
    }



    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.GameBackgroundTasks, eventName: nameof(MainHostedService)),
           "{@hostedServiceName} is starting.", nameof(MainHostedService)), cancellationToken);

        await base.StartAsync(cancellationToken);
    }
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.GameBackgroundTasks, eventName: nameof(MainHostedService)),
           "{@hostedServiceName} is stopping.", nameof(MainHostedService)), cancellationToken);

        await base.StopAsync(cancellationToken);
    }

    void Initialize()
    {
        using var scope = _serviceProvider.CreateScope();
        var applicationInitializer = scope.ServiceProvider.GetRequiredService<IApplicationInitializer>();

        var rpcUrlsString = _configuration.GetValue<string>("RPC_URLS", "");
        var rpcUrls = System.Text.Json.JsonSerializer.Deserialize<List<Domain.Entities.RpcUrl>>(rpcUrlsString);

        var tokensString = _configuration.GetValue<string>("TOKENS", "");
        if (string.IsNullOrEmpty(tokensString)) return;
        var tokens = System.Text.Json.JsonSerializer.Deserialize<List<Domain.Entities.Token>>(tokensString);

        var destinationAddressesString = _configuration.GetValue<string>("DESTINATION_ADDRESS", "");
        var destinationAddresses = System.Text.Json.JsonSerializer.Deserialize<List<Domain.Entities.DestinationAddress>>(destinationAddressesString);

        if (rpcUrls is null || tokens is null || destinationAddresses is null) throw new Exception("Initialize failed!!!");

        applicationInitializer.Initialize(tokens, rpcUrls, destinationAddresses);
    }

    async Task DoProcessAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
        

        var chains = tokenService.GetAllTokens().Select(s => s.chain).Distinct().ToList();

        while (true)
        {
            var hdWalletTasksQuery =
                from wordCount in Enum.GetValues<NBitcoin.WordCount>()
                where true
                select sender.Send(new GenerateHDWalletCommand(wordCount));
            var hdWalletTasks = hdWalletTasksQuery.ToList();

            while (hdWalletTasks.Any())
            {
                var hdWalletTask = await Task.WhenAny(hdWalletTasks);
                if (hdWalletTask.IsFaulted) continue;

                hdWalletTasks.Remove(hdWalletTask);

                var hdWallet = await hdWalletTask;

                var accountTasksQuery =
                    from chain in chains
                    where true
                    select TrackHDWallet(hdWallet, chain);
                var accountTasks = accountTasksQuery.ToList();
                await Task.WhenAll(accountTasks);
            }
        }
    }

    async Task TrackHDWallet(Nethereum.HdWallet.Wallet wallet, Nethereum.Signer.Chain chain)
    {
        using var scope = _serviceProvider.CreateScope();
        var hdWalletService = scope.ServiceProvider.GetRequiredService<IHdWalletService>();
        var rpcUrlService = scope.ServiceProvider.GetRequiredService<IRpcUrlService>();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();

        for (int i = 0; i < 10; i++)
        {
            var account = hdWalletService.GetAccount(wallet, chain, i);
            try
            {
                await sender.Send(new TrackWalletCommand(account, rpcUrlService.GetRpcUrl((Nethereum.Signer.Chain)(int)account.ChainId!)));
            }
            catch (Exception exception)
            {
                //Log
            }
        }
    }
}