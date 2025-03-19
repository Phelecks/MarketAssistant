using System.Diagnostics;
using BlockChain.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace BlockChain.MigrationWorker;

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
            await SeedDataAsync(context, stoppingToken);
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

    private static async Task SeedDataAsync(ApplicationDbContext context, CancellationToken stoppingToken)
    {
        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Seed the database
            await using var transaction = await context.Database.BeginTransactionAsync(stoppingToken);
            await TrySeedCustomersAsync(context, stoppingToken);
            await context.SaveChangesAsync(stoppingToken);
            await transaction.CommitAsync(stoppingToken);
        });
    }

    private static async Task TrySeedCustomersAsync(ApplicationDbContext context, CancellationToken stoppingToken)
    {
        if (!await context.Customers.AnyAsync(stoppingToken))
        {
            var customers = new List<Domain.Entities.Customer>
            {
                new() { WalletAddress = "Admin", Tracks = [new() { CustomerWalletAddress = "Admin", WalletAddress = "0x429B8474BD7308b7787d364985bB4b8eA7De1D47"}], Notifications = [new () { Identifier = "1292911958881734756", CustomerWalletAddress = "Admin", Type = Domain.Entities.Notification.NotificationType.Discord}] },
            };
            await context.Customers.AddRangeAsync(customers, stoppingToken);
        }
    }
}
