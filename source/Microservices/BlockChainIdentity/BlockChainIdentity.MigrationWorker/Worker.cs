using System.Diagnostics;
using BaseDomain.Enums;
using BlockChainIdentity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace BlockChainIdentity.MigrationWorker;

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

            if (context.Database.IsSqlServer())
            {
                var ensureDeleted = configuration.GetValue("ENSURE-DELETED-DATABASE-ON-STARTUP", false);
                if (ensureDeleted) await context.Database.EnsureDeletedAsync(stoppingToken);
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
            await TrySeedRolesAsync(context, stoppingToken);
            await TrySeedAdministratorWalletAsync(context, stoppingToken);
            await TrySeedResourcesAsync(context, stoppingToken);
            await TrySeedClientsAsync(context, stoppingToken);
            await TrySeedBaseParametersAsync(context, stoppingToken);
            await TrySeedRpcUrlsAsync(context, configuration, stoppingToken);
            await context.SaveChangesAsync(stoppingToken);
            await transaction.CommitAsync(stoppingToken);
        });
    }

    private static async Task TrySeedRolesAsync(ApplicationDbContext context, CancellationToken stoppingToken)
    {
        if (!await context.Roles.AnyAsync(stoppingToken))
        {
            await context.Roles.AddRangeAsync(
            [
                new() {
                    Title = "Administrators"
                },
                new() {
                    Title = "Users"
                }
            ], stoppingToken);

            await context.SaveChangesAsync(stoppingToken);
        }
    }
    private static async Task TrySeedAdministratorWalletAsync(ApplicationDbContext context, CancellationToken stoppingToken)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (!await context.Wallets.AnyAsync(stoppingToken))
        {
            var adminRole = await context.Roles.SingleAsync(exp => exp.Title.Equals("Administrators"), cancellationToken: stoppingToken);
            await context.Wallets.AddRangeAsync(
            [
                new() {
                    Address = !string.IsNullOrEmpty(environment) && (environment.Equals("Development") || environment.Contains("Test"))
                        ? "0xe1BA310dC3481EE3a242B1aDDbDE4049F70784B9"
                        : "0x599c632a9A37b749F7F14b7C7d842c42496d34C0",
                    ChainId = 137,
                    Created = DateTime.UtcNow,
                    WalletRoles =
                    [
                        new() {
                            Role = adminRole,
                            WalletAddress = !string.IsNullOrEmpty(environment) && (environment.Equals("Development") || environment.Contains("Test"))
                                ? "0xe1BA310dC3481EE3a242B1aDDbDE4049F70784B9"
                                : "0x599c632a9A37b749F7F14b7C7d842c42496d34C0",
                        }
                    ]
                }
            ], stoppingToken);

            await context.SaveChangesAsync(stoppingToken);
        }
    }
    private static async Task TrySeedResourcesAsync(ApplicationDbContext context, CancellationToken stoppingToken)
    {
        if (!await context.Resources.AnyAsync(stoppingToken))
        {
            await context.Resources.AddRangeAsync(
            [
                new()
                {
                    Title = "BlockChainIdentity"
                },
                new()
                {
                    Title = "Financial"
                },
                new()
                {
                    Title = "LogProcessor"
                },
                new()
                {
                    Title = "BlockProcessor"
                },
                new()
                {
                    Title = "BlockChain"
                },
                new()
                {
                    Title = "BlockChainTransfer"
                },
                new()
                {
                    Title = "Order"
                },
                new()
                {
                    Title = "Basket"
                },
                new()
                {
                    Title = "Catalog"
                },
                new()
                {
                    Title = "Customer"
                },
                new()
                {
                    Title = "Informing"
                }
            ], stoppingToken);
            await context.SaveChangesAsync(stoppingToken);
        }
    }
    private static async Task TrySeedClientsAsync(ApplicationDbContext context, CancellationToken stoppingToken)
    {
        if (!await context.Clients.AnyAsync(stoppingToken))
        {
            var BlockChainIdentityResource = await context.Resources.SingleAsync(exp => exp.Title.Equals("BlockChainIdentity"), stoppingToken);
            var FinancialResource = await context.Resources.SingleAsync(exp => exp.Title.Equals("Financial"), stoppingToken);
            var LogProcessorResource = await context.Resources.SingleAsync(exp => exp.Title.Equals("LogProcessor"), stoppingToken);
            var BlockProcessorResource = await context.Resources.SingleAsync(exp => exp.Title.Equals("BlockProcessor"), stoppingToken);
            var BlockChainResource = await context.Resources.SingleAsync(exp => exp.Title.Equals("BlockChain"), stoppingToken);
            var BlockChainTransferResource = await context.Resources.SingleAsync(exp => exp.Title.Equals("BlockChainTransfer"), stoppingToken);
            var OrderResource = await context.Resources.SingleAsync(exp => exp.Title.Equals("Order"), stoppingToken);
            var BasketResource = await context.Resources.SingleAsync(exp => exp.Title.Equals("Basket"), stoppingToken);
            var CatalogResource = await context.Resources.SingleAsync(exp => exp.Title.Equals("Catalog"), stoppingToken);
            var CustomerResource = await context.Resources.SingleAsync(exp => exp.Title.Equals("Customer"), stoppingToken);
            var InformingResource = await context.Resources.SingleAsync(exp => exp.Title.Equals("Informing"), stoppingToken);

            await context.Clients.AddRangeAsync(
            [
                new() {
                    ClientId = "adminPanelClientId",
                    ClientSecret = "!AdminPanel@Tricksfor#2024",
                    TokenLifeTimeInSeconds = 36000,
                    Enabled = true,
                    Statement = "You are about to connect to Tricksfor Admin panel.",
                    Uri = new Uri("https://marketassitantadmin.tricksfor.com"),
                    Version = "1",
                    ClientResources =
                    [
                        new() {
                            Resource = BlockChainIdentityResource
                        },
                        new() {
                            Resource = FinancialResource
                        },
                        new() {
                            Resource = LogProcessorResource
                        },
                        new() {
                            Resource = BlockProcessorResource
                        },
                        new() {
                            Resource = BlockChainResource
                        },
                        new() {
                            Resource = BlockChainTransferResource
                        },
                        new() {
                            Resource = OrderResource
                        },
                        new() {
                            Resource = BasketResource
                        },
                        new() {
                            Resource = CatalogResource
                        },
                        new() {
                            Resource = CustomerResource
                        },
                        new() {
                            Resource = InformingResource
                        }
                    ]
                },
                new() {
                    ClientId = "WebAppClientId",
                    ClientSecret = "(APP)&(Tricksfor)*2024",
                    TokenLifeTimeInSeconds = 43200,
                    Enabled = true,
                    Statement = "You are about to connect to marketassistant.tricksfor.com.",
                    Uri = new Uri("https://marketassistant.tricksfor.com"),
                    Version = "1",
                    ClientResources =
                    [
                        new() {
                            Resource = BlockChainIdentityResource
                        },
                        new() {
                            Resource = FinancialResource
                        },
                        new() {
                            Resource = LogProcessorResource
                        },
                        new() {
                            Resource = BlockProcessorResource
                        },
                        new() {
                            Resource = BlockChainResource
                        },
                        new() {
                            Resource = BlockChainTransferResource
                        },
                        new() {
                            Resource = OrderResource
                        },
                        new() {
                            Resource = BasketResource
                        },
                        new() {
                            Resource = CatalogResource
                        },
                        new() {
                            Resource = CustomerResource
                        },
                        new() {
                            Resource = InformingResource
                        },
                    ]
                }
            ], stoppingToken);

            await context.SaveChangesAsync(stoppingToken);
        }
    }
    private static async Task TrySeedBaseParametersAsync(ApplicationDbContext context, CancellationToken stoppingToken)
    {
        if (!await context.BaseParameters.AnyAsync(stoppingToken))
        {
            await context.BaseParameters.AddRangeAsync(
            [
                #region BlockChain Identity
                new()
                {
                    Category = BaseParameterCategory.BlockChainIdentityConfiguration,
                    Field = BaseParameterField.BlockChainIdentityDefaultGeneratedSiweMessageLifeTime,
                    Value = "60"
                }
                #endregion
            ], stoppingToken);

            await context.SaveChangesAsync(stoppingToken);
        }
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
                    new() { Chain = (Nethereum.Signer.Chain)rpcUrl.Chain, Uri = new Uri(rpcUrl.Url) }
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
}
