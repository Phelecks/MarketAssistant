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

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if(context.Database.IsSqlServer())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var ensureDeleted = configuration.GetValue("ENSURE_DELETED_DATABASE_ON_STARTUP", false);
                if(ensureDeleted) await context.Database.EnsureDeletedAsync(cancellationToken);
            }

            await EnsureDatabaseAsync(context, cancellationToken);
            await RunMigrationAsync(context, cancellationToken);
            await SeedDataAsync(context, cancellationToken);
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }

    private static async Task EnsureDatabaseAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        var dbCreator = context.GetService<IRelationalDatabaseCreator>();

        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Create the database if it does not exist.
            // Do this first so there is then a database to start a transaction against.
            if (!await dbCreator.ExistsAsync(cancellationToken))
            {
                await dbCreator.CreateAsync(cancellationToken);
            }
        });
    }

    private static async Task RunMigrationAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            await context.Database.MigrateAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }

    private static async Task SeedDataAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Seed the database
            await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            await TrySeedRpcUrlsAsync(context, cancellationToken);
            await TrySeedWalletAddresesAsync(context, cancellationToken);
            await TrySeedBlockProgressAsync(context, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }

    private static async Task TrySeedRpcUrlsAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        if (!await context.RpcUrls.AnyAsync(cancellationToken))
        {
            var records = new List<Domain.Entities.RpcUrl>
            {
                new() { Chain = Nethereum.Signer.Chain.Polygon, WaitIntervalOfBlockProgress = 10, BlockOfConfirmation = 3, Uri = new Uri("https://polygon-mainnet.g.alchemy.com/v2/22Jr03KTaxzY9R6szSsaYs2zumuPef9u"), }
            };

            if(!context.Database.IsSqlServer())
            {
                int index = 1;
                foreach(var record in records)
                    record.Id = index++;
            }

            await context.RpcUrls.AddRangeAsync(records, cancellationToken);
        }
    }

    private static async Task TrySeedWalletAddresesAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        if (!await context.WalletAddresses.AnyAsync(cancellationToken))
        {
            var walletAddresses = new List<Domain.Entities.WalletAddress>
            {
                new() { Address = "0x0B33Da9E1d07B72A8344948644eB5254919aa312" },
                new() { Address = "0x46b05Ea9031648E7F16F94e9876e73ab7d818633" },
                new() { Address = "0x429B8474BD7308b7787d364985bB4b8eA7De1D47" }
            };

            await context.WalletAddresses.AddRangeAsync(walletAddresses, cancellationToken);
        }
    }

    private static async Task TrySeedBlockProgressAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        if (!await context.BlockProgresses.AnyAsync(cancellationToken))
        {
            var records = new List<Domain.Entities.BlockProgress>
            {
                new() { Chain = Nethereum.Signer.Chain.Polygon, BlockNumber = 68654002, Status = Domain.Entities.BlockProgress.BlockProgressStatus.Processed}
            };

            if(!context.Database.IsSqlServer())
            {
                int index = 1;
                foreach(var record in records)
                    record.Id = index++;
            }

            await context.BlockProgresses.AddRangeAsync(records, cancellationToken);
        }
    }
}
