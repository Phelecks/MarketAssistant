using System.Diagnostics;
using BlockProcessor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace BlockProcessor.MigrationWorker;

public class Worker(IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            if(context.Database.IsSqlServer())
            {
                var ensureDeleted = configuration.GetValue("ENSURE-DELETED-DATABASE-ON-STARTUP", false);
                if(ensureDeleted) await context.Database.EnsureDeletedAsync(stoppingToken);
            }

            await EnsureDatabaseAsync(context, stoppingToken);
            await RunMigrationAsync(context, stoppingToken);
            await SeedDataAsync(context, configuration, stoppingToken);
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }

    private static async Task EnsureDatabaseAsync(ApplicationDbContext context, CancellationToken stoppingToken)
    {
        var dbCreator = context.GetService<IRelationalDatabaseCreator>();

        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Create the database if it does not exist.
            // Do this first so there is then a database to start a transaction against.
            if (!await dbCreator.ExistsAsync(stoppingToken))
            {
                await dbCreator.CreateAsync(stoppingToken);
            }
        });
    }

    private static async Task RunMigrationAsync(ApplicationDbContext context, CancellationToken stoppingToken)
    {
        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            await using var transaction = await context.Database.BeginTransactionAsync(stoppingToken);
            await context.Database.MigrateAsync(stoppingToken);
            await transaction.CommitAsync(stoppingToken);
        });
    }

    private static async Task SeedDataAsync(ApplicationDbContext context, IConfiguration configuration, CancellationToken stoppingToken)
    {
        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Seed the database
            await using var transaction = await context.Database.BeginTransactionAsync(stoppingToken);
            await TrySeedRpcUrlsAsync(context, configuration, stoppingToken);
            await TrySeedWalletAddresesAsync(context, stoppingToken);
            await context.SaveChangesAsync(stoppingToken);
            await transaction.CommitAsync(stoppingToken);
        });
    }

    private sealed record RpcUrl(int Chain, string Url);
    private static async Task TrySeedRpcUrlsAsync(ApplicationDbContext context, IConfiguration configuration, CancellationToken stoppingToken)
    {
        var rpcUrlsString = configuration.GetValue<string>("RPC-URLS");
        if(string.IsNullOrEmpty(rpcUrlsString)) throw new InvalidOperationException("RPC-URLS is not set in configuration");
        var rpcUrls = System.Text.Json.JsonSerializer.Deserialize<List<RpcUrl>>(rpcUrlsString) ?? throw new InvalidOperationException("RPC-URLS is not set or not in correct format in configuration");
        if (!await context.RpcUrls.AnyAsync(stoppingToken))
        {
            var records = new List<Domain.Entities.RpcUrl>();
            foreach(var rpcUrl in rpcUrls)
            {
                records.Add(
                    new() { Chain = (Nethereum.Signer.Chain)rpcUrl.Chain, WaitInterval = 100, BlockOfConfirmation = 3, Uri = new Uri(rpcUrl.Url) }
                );
            }

            if(!context.Database.IsSqlServer())
            {
                int index = 1;
                foreach(var record in records)
                    record.Id = index++;
            }

            await context.RpcUrls.AddRangeAsync(records, stoppingToken);
        }
    }
    
    private static async Task TrySeedWalletAddresesAsync(ApplicationDbContext context, CancellationToken stoppingToken)
    {
        if (!await context.WalletAddresses.AnyAsync(stoppingToken))
        {
            var walletAddresses = new List<Domain.Entities.WalletAddress>
            {
                new() { Address = "0x0B33Da9E1d07B72A8344948644eB5254919aa312" },
                new() { Address = "0x46b05Ea9031648E7F16F94e9876e73ab7d818633" },
                new() { Address = "0x429B8474BD7308b7787d364985bB4b8eA7De1D47" },
                new() { Address = "0x6b2F207484054773D9DE78a45E6f94683bfE40fa" }
            };

            await context.WalletAddresses.AddRangeAsync(walletAddresses, stoppingToken);
        }
    }
}
