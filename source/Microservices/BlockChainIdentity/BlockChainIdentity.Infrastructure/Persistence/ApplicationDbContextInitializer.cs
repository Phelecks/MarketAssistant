using BlockChainIdentity.Domain.Entities;
using LoggerService.Helpers;
using Microsoft.EntityFrameworkCore;
using BaseInfrastructure.Common;
using Microsoft.Extensions.Logging;
using BaseDomain.Enums;
using Microsoft.Extensions.Configuration;

namespace BlockChainIdentity.Infrastructure.Persistence;

public class ApplicationDbContextInitializer : IApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;
    private readonly string? _environment;
    private readonly IConfiguration _configuration;

    public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger, ApplicationDbContext context, IConfiguration configuration)
    {
        _logger = logger;
        _context = context;
        _environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        _configuration = configuration;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (!string.IsNullOrEmpty(environment) && environment.Equals("Development"))
                {
                    var ensureDeleted = _configuration.GetValue("ENSURE_DELETED_DATABASE_ON_STARTUP", false);
                    if(ensureDeleted) await _context.Database.EnsureDeletedAsync();
                }
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _ = Task.Run(() => _logger.LogError(
                eventId: EventTool.GetEventInformation(eventType: EventType.BlockChainLogProcessorException, eventName: "Initialize database"),
                exception: ex, message: "An error occurred while initializing the database"));
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedBaseParametersAsync();
            await TrySeedRolesAsync();
            await TrySeedAdministratorWalletAsync();
            await TrySeedResourcesAsync();
            await TrySeedClientsAsync();
        }
        catch (Exception ex)
        {
            _ = Task.Run(() => _logger.LogError(
                eventId: EventTool.GetEventInformation(eventType: EventType.BlockChainLogProcessorException, eventName: "Seed database"),
                exception: ex, message: "An error occurred while seeding the database."));
            throw;
        }
    }

    private async Task TrySeedRolesAsync()
    {
        // Default data
        // Seed, if necessary
        if (!_context.roles.Any())
        {
            await _context.roles.AddRangeAsync(new List<Domain.Entities.Role>
            {
                new Domain.Entities.Role
                {
                    title = "Administrators"
                },
                new Domain.Entities.Role
                {
                    title = "Users"
                }
            });
            
            await _context.SaveChangesAsync();
        }
    }
    private async Task TrySeedAdministratorWalletAsync()
    {
        // Default data
        // Seed, if necessary
        if (!_context.wallets.Any())
        {
            var adminRole = await _context.roles.SingleAsync(exp => exp.title.Equals("Administrators"));
            await _context.wallets.AddRangeAsync(new List<Domain.Entities.Wallet>
            {
                new Domain.Entities.Wallet
                {
                    address = !string.IsNullOrEmpty(_environment) && (_environment.Equals("Development") || _environment.Contains("Test"))
                        ? "0xe1BA310dC3481EE3a242B1aDDbDE4049F70784B9"
                        : "0x599c632a9A37b749F7F14b7C7d842c42496d34C0",
                    chainId = 137,
                    created = DateTime.UtcNow,
                    walletRoles = new List<WalletRole> 
                    {
                        new WalletRole
                        {
                            role = adminRole,
                            walletAddress = !string.IsNullOrEmpty(_environment) && (_environment.Equals("Development") || _environment.Contains("Test"))
                                ? "0xe1BA310dC3481EE3a242B1aDDbDE4049F70784B9"
                                : "0x599c632a9A37b749F7F14b7C7d842c42496d34C0",
                        }
                    }
                }
            });

            await _context.SaveChangesAsync();
        }
    }
    private async Task TrySeedResourcesAsync()
    {
        if (!_context.resources.Any())
        {
            await _context.resources.AddRangeAsync(new List<Resource>
                    {
                        new Resource
                            {
                                title = "BlockChainIdentity"
                            },
                         new Resource
                            {
                                title = "Financial"
                            },
                         new Resource
                            {
                                title = "BlockChainLogProcessor"
                            },
                        new Resource
                            {
                                title = "BlockChainPayment"
                            },
                        new Resource
                            {
                                title = "BlockChain"
                            },
                        new Resource
                            {
                                title = "BlockChainTransfer"
                            },
                        new Resource
                            {
                                title = "Order"
                            },
                        new Resource
                            {
                                title = "Basket"
                            },
                        new Resource
                            {
                                title = "Catalog"
                            },
                        new Resource
                            {
                                title = "Customer"
                            },
                        new Resource
                            {
                                title = "Informing"
                            }
            });
        }
        await _context.SaveChangesAsync();
    }
    private async Task TrySeedClientsAsync()
    {

        // Default data
        // Seed, if necessary
        if (!_context.clients.Any())
        {
            var BlockChainIdentityResource = await _context.resources.SingleAsync(exp => exp.title.Equals("BlockChainIdentity"));
            var FinancialResource = await _context.resources.SingleAsync(exp => exp.title.Equals("Financial"));
            var BlockChainLogProcessorResource = await _context.resources.SingleAsync(exp => exp.title.Equals("BlockChainLogProcessor"));
            var BlockChainPaymentResource = await _context.resources.SingleAsync(exp => exp.title.Equals("BlockChainPayment"));
            var BlockChainResource = await _context.resources.SingleAsync(exp => exp.title.Equals("BlockChain"));
            var BlockChainTransferResource = await _context.resources.SingleAsync(exp => exp.title.Equals("BlockChainTransfer"));
            var OrderResource = await _context.resources.SingleAsync(exp => exp.title.Equals("Order"));
            var BasketResource = await _context.resources.SingleAsync(exp => exp.title.Equals("Basket"));
            var CatalogResource = await _context.resources.SingleAsync(exp => exp.title.Equals("Catalog"));
            var CustomerResource = await _context.resources.SingleAsync(exp => exp.title.Equals("Customer"));
            var InformingResource = await _context.resources.SingleAsync(exp => exp.title.Equals("Informing"));

            await _context.clients.AddRangeAsync(new List<Client>
            {
                new Client
                {
                    clientId = "adminPanelClientId",
                    clientSecret = "!AdminPanel@Tricksfor#2024",
                    tokenLifeTimeInSeconds = 36000,
                    enabled = true,
                    statement = "You are about to connect to Tricksfor Admin panel.",
                    uri = new Uri("https://marketassitantadmin.tricksfor.com"),
                    version = "1",
                    clientResources = new List<ClientResource>
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
                    clientId = "WebAppClientId",
                    clientSecret = "(APP)&(Tricksfor)*2024",
                    tokenLifeTimeInSeconds = 43200,
                    enabled = true,
                    statement = "You are about to connect to marketassistant.tricksfor.com.",
                    uri = new Uri("https://marketassistant.tricksfor.com"),
                    version = "1",
                    clientResources = new List<ClientResource>
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
            });

            await _context.SaveChangesAsync();
        }
    }

    private async Task TrySeedBaseParametersAsync()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
       // Default data
       // Seed, if necessary
       if (!_context.baseParameters.Any())
       {
           await _context.baseParameters.AddRangeAsync(new List<BaseParameter>
           {
               #region BlockChain Identity
                new()
                {
                    category = BaseParameterCategory.BlockChainIdentityConfiguration,
                    field = BaseParameterField.BlockChainIdentityDefaultGeneratedSiweMessageLifeTime,
                    value = "60"
                },
                new()
                {
                    category = BaseParameterCategory.BlockChainIdentityConfiguration,
                    field = BaseParameterField.BlockChainIdentityPolygonMainNetRpcUrl,
                    value = !string.IsNullOrEmpty(environment) && environment.Contains("Development")
                        ? "https://polygon-mainnet.g.alchemy.com/v2/22Jr03KTaxzY9R6szSsaYs2zumuPef9u"
                        : !string.IsNullOrEmpty(environment) && environment.Contains("Staging")
                            ? "https://polygon-mainnet.g.alchemy.com/v2/obsDhx84u8_cHMhDWIsXqi4BpVyrLw3E"
                            :"https://polygon-mainnet.g.alchemy.com/v2/qt44--X9fNPny4QLv8Lx72ICeJgr-b8p"
                },
                new()
                {
                    category = BaseParameterCategory.BlockChainIdentityConfiguration,
                    field = BaseParameterField.BlockChainIdentityPolygonTestNetRpcUrl,
                    value = "https://polygon-amoy.g.alchemy.com/v2/obsDhx84u8_cHMhDWIsXqi4BpVyrLw3E"
                },
                #endregion
           });
            
           await _context.SaveChangesAsync();
       }
    }
}
