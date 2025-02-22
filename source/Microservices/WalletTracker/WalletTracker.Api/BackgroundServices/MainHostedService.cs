using BaseApplication.Interfaces;
using BlockChainHDWalletHelper.Interfaces;
using LoggerService.Helpers;
using MediatR;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using WalletTracker.Application.Interfaces;
using WalletTracker.Application.Track.Commands.TrackWallet;

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
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (!string.IsNullOrEmpty(environment) && environment.Contains("Test")) return;

        while (!cancellationToken.IsCancellationRequested)
        {
            Initialize();

            using (var scope = _serviceProvider.CreateScope())
            {
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                var hdWalletService = scope.ServiceProvider.GetRequiredService<IHdWalletService>();

                foreach (var wordCount in Enum.GetValues<NBitcoin.WordCount>())
                {
                    var hdWallet = hdWalletService.GenerateWallet("", wordCount);
                    while (true)
                    {
                        var account = hdWalletService.GetAccount(hdWallet,)
                    }
                }
                
                await sender.Send(new TrackWalletCommand(), cancellationToken);
                

                var intervalsInMinutes = 1;
                _ = Task.Run(() => _logger.LogInformation(
                     eventId: EventTool.GetEventInformation(eventType: EventType.GameBackgroundTasks, eventName: $"{nameof(MainHostedService)}"),
                     "{@hostedServiceName} is delaying for {@delay} minutes.", nameof(MainHostedService), intervalsInMinutes), cancellationToken);
                await Task.Delay(TimeSpan.FromMinutes(intervalsInMinutes), cancellationToken);
            }
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
        if(string.IsNullOrEmpty(tokensString)) return;
        var tokens = System.Text.Json.JsonSerializer.Deserialize<List<Domain.Entities.Token>>(tokensString);

        var destinationAddressesString = _configuration.GetValue<string>("DESTINATION_ADDRESS", "");
        var destinationAddresses = System.Text.Json.JsonSerializer.Deserialize<List<Domain.Entities.DestinationAddress>>(destinationAddressesString);

        if(rpcUrls is null || tokens is null || destinationAddresses is null) throw new Exception("Initialize failed!!!");

        applicationInitializer.Initialize(tokens, rpcUrls, destinationAddresses);
    }
}
