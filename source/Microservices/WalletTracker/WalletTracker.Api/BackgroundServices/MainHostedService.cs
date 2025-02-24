using BlockChainHDWalletHelper.Interfaces;
using LoggerService.Helpers;
using MediatR;
using Microsoft.Extensions.Configuration;
using WalletTracker.Application.Interfaces;
using WalletTracker.Application.Track.Commands.TrackWallet;
using WalletTracker.Application.Wallet.Commands.GenerateHDWallet;
using WalletTracker.Infrastructure.Services;

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

            try
            {
                await DoProcessAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                _ = Task.Run(() => _logger.LogError(
                     eventId: EventTool.GetEventInformation(eventType: EventType.GameBackgroundTasks, eventName: $"{nameof(MainHostedService)}"),
                     exception, exception.Message, cancellationToken), cancellationToken);
            }
            

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

        var rpcUrlsString = _configuration.GetValue<string>("RPC_URLS", System.Text.Json.JsonSerializer.Serialize(new List<Domain.Entities.RpcUrl>
        {
            new Domain.Entities.RpcUrl(1, Nethereum.Signer.Chain.MainNet, "https://eth-mainnet.g.alchemy.com/v2/22Jr03KTaxzY9R6szSsaYs2zumuPef9u"),
            new Domain.Entities.RpcUrl(2, Nethereum.Signer.Chain.Polygon, "https://polygon-mainnet.g.alchemy.com/v2/22Jr03KTaxzY9R6szSsaYs2zumuPef9u"),
            new Domain.Entities.RpcUrl(3, Nethereum.Signer.Chain.Arbitrum, "https://arb-mainnet.g.alchemy.com/v2/22Jr03KTaxzY9R6szSsaYs2zumuPef9u"),
            new Domain.Entities.RpcUrl(4, Nethereum.Signer.Chain.Binance, "https://bnb-mainnet.g.alchemy.com/v2/22Jr03KTaxzY9R6szSsaYs2zumuPef9u"),
            new Domain.Entities.RpcUrl(5, Nethereum.Signer.Chain.Avalanche, "https://avax-mainnet.g.alchemy.com/v2/22Jr03KTaxzY9R6szSsaYs2zumuPef9u"),
            new Domain.Entities.RpcUrl(6, Nethereum.Signer.Chain.Optimism, "https://opt-mainnet.g.alchemy.com/v2/22Jr03KTaxzY9R6szSsaYs2zumuPef9u"),
        }));
        var rpcUrls = System.Text.Json.JsonSerializer.Deserialize<List<Domain.Entities.RpcUrl>>(rpcUrlsString);

        var tokensString = _configuration.GetValue<string>("TOKENS", System.Text.Json.JsonSerializer.Serialize(new List<Domain.Entities.Token>
        {
            new Domain.Entities.Token(id: 1, "ETH", Nethereum.Signer.Chain.MainNet, BaseDomain.Enums.BlockChainEnums.TokenType.Main, true, null, 18 ),
            new Domain.Entities.Token(id: 2, symbol: "USDT", chain: Nethereum.Signer.Chain.MainNet, enabled: true, tokenType: BaseDomain.Enums.BlockChainEnums.TokenType.Erc20, contractAddress: BaseDomain.Helpers.SmartContractHelper.GetUsdtContractAddress(Nethereum.Signer.Chain.Polygon), decimals: 6 ),
            new Domain.Entities.Token(id: 3, symbol: "USDC", chain: Nethereum.Signer.Chain.MainNet, enabled: true, tokenType: BaseDomain.Enums.BlockChainEnums.TokenType.Erc20, contractAddress: BaseDomain.Helpers.SmartContractHelper.GetUsdcContractAddress(Nethereum.Signer.Chain.Polygon), decimals: 6 ),
            new Domain.Entities.Token(id: 4, symbol: "POL", chain: Nethereum.Signer.Chain.Polygon, enabled: true, tokenType: BaseDomain.Enums.BlockChainEnums.TokenType.Main, contractAddress: null, decimals: 18 ),
            new Domain.Entities.Token(id: 5, symbol: "USDT", chain: Nethereum.Signer.Chain.Polygon, enabled: true, tokenType: BaseDomain.Enums.BlockChainEnums.TokenType.Erc20, contractAddress: BaseDomain.Helpers.SmartContractHelper.GetUsdtContractAddress(Nethereum.Signer.Chain.Polygon), decimals: 6 ),
            new Domain.Entities.Token(id: 6, symbol: "USDC", chain: Nethereum.Signer.Chain.Polygon, enabled: true, tokenType: BaseDomain.Enums.BlockChainEnums.TokenType.Erc20, contractAddress: BaseDomain.Helpers.SmartContractHelper.GetUsdcContractAddress(Nethereum.Signer.Chain.Polygon), decimals: 6 ),
            new Domain.Entities.Token(id: 7, symbol: "ARB", chain: Nethereum.Signer.Chain.Arbitrum, enabled: true, tokenType: BaseDomain.Enums.BlockChainEnums.TokenType.Main, contractAddress: null, decimals: 18 ),
            new Domain.Entities.Token(id: 8, symbol: "USDT", chain: Nethereum.Signer.Chain.Arbitrum, enabled: true, tokenType: BaseDomain.Enums.BlockChainEnums.TokenType.Erc20, contractAddress: BaseDomain.Helpers.SmartContractHelper.GetUsdtContractAddress(Nethereum.Signer.Chain.Arbitrum), decimals: 6 ),
            new Domain.Entities.Token(id: 9, symbol: "USDC", chain: Nethereum.Signer.Chain.Arbitrum, enabled: true, tokenType: BaseDomain.Enums.BlockChainEnums.TokenType.Erc20, contractAddress: BaseDomain.Helpers.SmartContractHelper.GetUsdcContractAddress(Nethereum.Signer.Chain.Arbitrum), decimals: 6 ),

            new Domain.Entities.Token(id: 10, symbol: "BNB", chain: Nethereum.Signer.Chain.Binance, enabled: true, tokenType: BaseDomain.Enums.BlockChainEnums.TokenType.Main, contractAddress: null, decimals: 18 ),
            new Domain.Entities.Token(id: 11, symbol: "USDT", chain: Nethereum.Signer.Chain.Binance, enabled: true, tokenType: BaseDomain.Enums.BlockChainEnums.TokenType.Erc20, contractAddress: BaseDomain.Helpers.SmartContractHelper.GetUsdtContractAddress(Nethereum.Signer.Chain.Binance), decimals: 6 ),
            new Domain.Entities.Token(id: 12, symbol: "USDC", chain: Nethereum.Signer.Chain.Binance, enabled: true, tokenType: BaseDomain.Enums.BlockChainEnums.TokenType.Erc20, contractAddress: BaseDomain.Helpers.SmartContractHelper.GetUsdcContractAddress(Nethereum.Signer.Chain.Binance), decimals: 6 ),

            new Domain.Entities.Token(id: 13, symbol: "AVAX", chain: Nethereum.Signer.Chain.Avalanche, enabled: true, tokenType: BaseDomain.Enums.BlockChainEnums.TokenType.Main, contractAddress: null, decimals: 18 ),
            new Domain.Entities.Token(id: 14, symbol: "USDT", chain: Nethereum.Signer.Chain.Avalanche, enabled: true, tokenType: BaseDomain.Enums.BlockChainEnums.TokenType.Erc20, contractAddress: BaseDomain.Helpers.SmartContractHelper.GetUsdtContractAddress(Nethereum.Signer.Chain.Avalanche), decimals: 6 ),
            new Domain.Entities.Token(id: 15, symbol: "USDC", chain: Nethereum.Signer.Chain.Avalanche, enabled: true, tokenType: BaseDomain.Enums.BlockChainEnums.TokenType.Erc20, contractAddress: BaseDomain.Helpers.SmartContractHelper.GetUsdcContractAddress(Nethereum.Signer.Chain.Avalanche), decimals: 6 ),

            new Domain.Entities.Token(id: 16, symbol: "OP", chain: Nethereum.Signer.Chain.Optimism, enabled: true, tokenType: BaseDomain.Enums.BlockChainEnums.TokenType.Main, contractAddress: null, decimals: 18 ),
            new Domain.Entities.Token(id: 17, symbol: "USDT", chain: Nethereum.Signer.Chain.Optimism, enabled: true, tokenType: BaseDomain.Enums.BlockChainEnums.TokenType.Erc20, contractAddress: BaseDomain.Helpers.SmartContractHelper.GetUsdtContractAddress(Nethereum.Signer.Chain.Optimism), decimals: 6 ),
            new Domain.Entities.Token(id: 18, symbol: "USDC", chain: Nethereum.Signer.Chain.Optimism, enabled: true, tokenType: BaseDomain.Enums.BlockChainEnums.TokenType.Erc20, contractAddress: BaseDomain.Helpers.SmartContractHelper.GetUsdcContractAddress(Nethereum.Signer.Chain.Optimism), decimals: 6 ),
        }));
        if (string.IsNullOrEmpty(tokensString)) return;
        var tokens = System.Text.Json.JsonSerializer.Deserialize<List<Domain.Entities.Token>>(tokensString);

        var destinationAddressesString = _configuration.GetValue<string>("DESTINATION_ADDRESS", System.Text.Json.JsonSerializer.Serialize(new List<Domain.Entities.DestinationAddress>
        {
            new Domain.Entities.DestinationAddress( id: 1, chain: Nethereum.Signer.Chain.MainNet, address: "0xEAbE38EEB8813ec8D90C594aC388267F26F9C559" ),
            new Domain.Entities.DestinationAddress( id: 2, chain: Nethereum.Signer.Chain.Polygon, address: "0xEAbE38EEB8813ec8D90C594aC388267F26F9C559" ),
            new Domain.Entities.DestinationAddress( id: 3, chain: Nethereum.Signer.Chain.Arbitrum, address: "0xEAbE38EEB8813ec8D90C594aC388267F26F9C559" ),
            new Domain.Entities.DestinationAddress( id: 4, chain: Nethereum.Signer.Chain.Binance, address: "0xEAbE38EEB8813ec8D90C594aC388267F26F9C559" ),
            new Domain.Entities.DestinationAddress( id: 5, chain: Nethereum.Signer.Chain.Avalanche, address: "0xEAbE38EEB8813ec8D90C594aC388267F26F9C559" ),
            new Domain.Entities.DestinationAddress( id: 6, chain: Nethereum.Signer.Chain.Optimism, address: "0xEAbE38EEB8813ec8D90C594aC388267F26F9C559" )
        }));
        var destinationAddresses = System.Text.Json.JsonSerializer.Deserialize<List<Domain.Entities.DestinationAddress>>(destinationAddressesString);

        if (rpcUrls is null || tokens is null || destinationAddresses is null) throw new Exception("Initialize failed!!!");

        applicationInitializer.Initialize(tokens, rpcUrls, destinationAddresses);
    }

    async Task DoProcessAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        var hdWalletService = scope.ServiceProvider.GetRequiredService<IHdWalletService>();

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

                //var accountTasksQuery =
                //    from chain in chains
                //    where true
                //    select TrackHDWallet(hdWallet, chain);
                //var accountTasks = accountTasksQuery.ToList();
                //await Task.WhenAll(accountTasks);


                var accounts = new List<Nethereum.Web3.Accounts.Account>();
                for (int i = 0; i < 10; i++)
                    accounts.Add(hdWalletService.GetAccount(hdWallet, i));
                var accountTasksQuery =
                    from account in accounts
                    where true
                    select TrackWallet(account);
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
                await sender.Send(new TrackWalletCommand(account, (Nethereum.Signer.Chain)(int)account.ChainId!, rpcUrlService.GetRpcUrl((Nethereum.Signer.Chain)(int)account.ChainId!)));
            }
            catch (Exception exception)
            {
                //Log
            }
        }
    }

    async Task TrackWallet(Nethereum.Web3.Accounts.Account account)
    {
        using var scope = _serviceProvider.CreateScope();
        var rpcUrlService = scope.ServiceProvider.GetRequiredService<IRpcUrlService>();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();

        var chains = tokenService.GetAllTokens().Select(s => s.chain).Distinct().ToList();

        foreach (var chain in chains)
        {
            try
            {
                await sender.Send(new TrackWalletCommand(account, chain, rpcUrlService.GetRpcUrl(chain)));
            }
            catch (Exception exception)
            {
                //Log
            }
        }
    }
}