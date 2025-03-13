using System.Diagnostics;
using BaseDomain.Enums;
using BlockChainIdentity.Domain.Entities;
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
            await TrySeedRolesAsync(context, stoppingToken);
            await TrySeedAdministratorWalletAsync(context, stoppingToken);
            await TrySeedResourcesAsync(context, stoppingToken);
            await TrySeedClientsAsync(context, stoppingToken);
            await TrySeedBaseParametersAsync(context, stoppingToken);
            await context.SaveChangesAsync(stoppingToken);
            await transaction.CommitAsync(stoppingToken);
        });
    }

    private static async Task TrySeedRolesAsync(ApplicationDbContext context, CancellationToken stoppingToken)
    {
        if (!await context.Roles.AnyAsync(stoppingToken))
        {
            await context.Roles.AddRangeAsync(new List<Domain.Entities.Role>
            {
                new Domain.Entities.Role
                {
                    title = "Administrators"
                },
                new Domain.Entities.Role
                {
                    title = "Users"
                }
            }, stoppingToken);

            await context.SaveChangesAsync(stoppingToken);
        }
    }
    private static async Task TrySeedAdministratorWalletAsync(ApplicationDbContext context, CancellationToken stoppingToken)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (!await context.Wallets.AnyAsync(stoppingToken))
        {
            var adminRole = await context.Roles.SingleAsync(exp => exp.title.Equals("Administrators"), cancellationToken: stoppingToken);
            await context.Wallets.AddRangeAsync(new List<Domain.Entities.Wallet>
            {
                new() {
                    Address = !string.IsNullOrEmpty(environment) && (environment.Equals("Development") || environment.Contains("Test"))
                        ? "0xe1BA310dC3481EE3a242B1aDDbDE4049F70784B9"
                        : "0x599c632a9A37b749F7F14b7C7d842c42496d34C0",
                    ChainId = 137,
                    Created = DateTime.UtcNow,
                    WalletRoles = new List<Domain.Entities.WalletRole>
                    {
                        new Domain.Entities.WalletRole
                        {
                            role = adminRole,
                            walletAddress = !string.IsNullOrEmpty(environment) && (environment.Equals("Development") || environment.Contains("Test"))
                                ? "0xe1BA310dC3481EE3a242B1aDDbDE4049F70784B9"
                                : "0x599c632a9A37b749F7F14b7C7d842c42496d34C0",
                        }
                    }
                }
            });

            await context.SaveChangesAsync(stoppingToken);
        }
    }
    private static async Task TrySeedResourcesAsync(ApplicationDbContext context, CancellationToken stoppingToken)
    {
        if (!await context.Resources.AnyAsync(stoppingToken))
        {
            await context.Resources.AddRangeAsync(new List<Resource>
            {
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
                    Title = "BlockChainLogProcessor"
                },
                new()
                    {
                        Title = "BlockChainPayment"
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
            }, stoppingToken);
            await context.SaveChangesAsync(stoppingToken);
        }
    }
    private static async Task TrySeedClientsAsync(ApplicationDbContext context, CancellationToken stoppingToken)
    {

        // Default data
        // Seed, if necessary
        if (!await context.Clients.AnyAsync(stoppingToken))
        {
            var BlockChainIdentityResource = await context.Resources.SingleAsync(exp => exp.Title.Equals("BlockChainIdentity"));
            var FinancialResource = await context.Resources.SingleAsync(exp => exp.Title.Equals("Financial"));
            var BlockChainLogProcessorResource = await context.Resources.SingleAsync(exp => exp.Title.Equals("BlockChainLogProcessor"));
            var BlockChainPaymentResource = await context.Resources.SingleAsync(exp => exp.Title.Equals("BlockChainPayment"));
            var BlockChainResource = await context.Resources.SingleAsync(exp => exp.Title.Equals("BlockChain"));
            var BlockChainTransferResource = await context.Resources.SingleAsync(exp => exp.Title.Equals("BlockChainTransfer"));
            var OrderResource = await context.Resources.SingleAsync(exp => exp.Title.Equals("Order"));
            var BasketResource = await context.Resources.SingleAsync(exp => exp.Title.Equals("Basket"));
            var CatalogResource = await context.Resources.SingleAsync(exp => exp.Title.Equals("Catalog"));
            var CustomerResource = await context.Resources.SingleAsync(exp => exp.Title.Equals("Customer"));
            var InformingResource = await context.Resources.SingleAsync(exp => exp.Title.Equals("Informing"));

            await context.Clients.AddRangeAsync(new List<Client>
            {
                new() {
                    ClientId = "adminPanelClientId",
                    ClientSecret = "!AdminPanel@Tricksfor#2024",
                    TokenLifeTimeInSeconds = 36000,
                    Enabled = true,
                    Statement = "You are about to connect to Tricksfor Admin panel.",
                    Uri = new Uri("https://marketassitantadmin.tricksfor.com"),
                    Version = "1",
                    ClientResources = new List<ClientResource>
                    {
                        new ClientResource
                        {
                            resource = BlockChainIdentityResource
                        },
                        new ClientResource
                        {
                            resource = FinancialResource
                        },
                        new ClientResource
                        {
                            resource = BlockChainLogProcessorResource
                        },
                        new ClientResource
                        {
                            resource = BlockChainPaymentResource
                        },
                        new ClientResource
                        {
                            resource = BlockChainResource
                        },
                        new ClientResource
                        {
                            resource = BlockChainTransferResource
                        },
                        new ClientResource
                        {
                            resource = OrderResource
                        },
                        new ClientResource
                        {
                            resource = BasketResource
                        },
                        new ClientResource
                        {
                            resource = CatalogResource
                        },
                        new ClientResource
                        {
                            resource = CustomerResource
                        },
                        new ClientResource
                        {
                            resource = InformingResource
                        }
                    }
                },
                new Client
                {
                    ClientId = "WebAppClientId",
                    ClientSecret = "(APP)&(Tricksfor)*2024",
                    TokenLifeTimeInSeconds = 43200,
                    Enabled = true,
                    Statement = "You are about to connect to marketassistant.tricksfor.com.",
                    Uri = new Uri("https://marketassistant.tricksfor.com"),
                    Version = "1",
                    ClientResources = new List<ClientResource>
                    {
                        new ClientResource
                        {
                            resource = BlockChainIdentityResource
                        },
                        new ClientResource
                        {
                            resource = FinancialResource
                        },
                        new ClientResource
                        {
                            resource = BlockChainLogProcessorResource
                        },
                        new ClientResource
                        {
                            resource = BlockChainPaymentResource
                        },
                        new ClientResource
                        {
                            resource = BlockChainResource
                        },
                        new ClientResource
                        {
                            resource = BlockChainTransferResource
                        },
                        new ClientResource
                        {
                            resource = OrderResource
                        },
                        new ClientResource
                        {
                            resource = BasketResource
                        },
                        new ClientResource
                        {
                            resource = CatalogResource
                        },
                        new ClientResource
                        {
                            resource = CustomerResource
                        },
                        new ClientResource
                        {
                            resource = InformingResource
                        },
                    }
                }
            }, stoppingToken);

            await context.SaveChangesAsync(stoppingToken);
        }
    }
    private static async Task TrySeedBaseParametersAsync(ApplicationDbContext context, CancellationToken stoppingToken)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        // Default data
        // Seed, if necessary
        if (!await context.BaseParameters.AnyAsync(stoppingToken))
        {
            await context.BaseParameters.AddRangeAsync(new List<BaseParameter>
            {
                #region BlockChain Identity
                new()
                {
                    Category = BaseParameterCategory.BlockChainIdentityConfiguration,
                    Field = BaseParameterField.BlockChainIdentityDefaultGeneratedSiweMessageLifeTime,
                    Value = "60"
                },
                new()
                {
                    Category = BaseParameterCategory.BlockChainIdentityConfiguration,
                    Field = BaseParameterField.BlockChainIdentityPolygonMainNetRpcUrl,
                    Value = !string.IsNullOrEmpty(environment) && environment.Contains("Development")
                        ? "https://polygon-mainnet.g.alchemy.com/v2/22Jr03KTaxzY9R6szSsaYs2zumuPef9u"
                        : !string.IsNullOrEmpty(environment) && environment.Contains("Staging")
                            ? "https://polygon-mainnet.g.alchemy.com/v2/obsDhx84u8_cHMhDWIsXqi4BpVyrLw3E"
                            :"https://polygon-mainnet.g.alchemy.com/v2/qt44--X9fNPny4QLv8Lx72ICeJgr-b8p"
                },
                new()
                {
                    Category = BaseParameterCategory.BlockChainIdentityConfiguration,
                    Field = BaseParameterField.BlockChainIdentityPolygonTestNetRpcUrl,
                    Value = "https://polygon-amoy.g.alchemy.com/v2/obsDhx84u8_cHMhDWIsXqi4BpVyrLw3E"
                },
                #endregion
            }, stoppingToken);

            await context.SaveChangesAsync(stoppingToken);
        }
    }
}
