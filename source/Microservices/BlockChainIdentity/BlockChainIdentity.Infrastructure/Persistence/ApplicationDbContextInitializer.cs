using BlockChainIdentity.Domain.Entities;
using LoggerService.Helpers;
using Microsoft.EntityFrameworkCore;
using BaseInfrastructure.Common;
using Microsoft.Extensions.Logging;

namespace BlockChainIdentity.Infrastructure.Persistence;

public class ApplicationDbContextInitializer : IApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;
    private readonly string? _environment;

    public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
        _environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                
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
                                title = "Kernel"
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
                                title = "CustomerClub"
                            },
                        new Resource
                            {
                                title = "Coin"
                            },
                        new Resource
                            {
                                title = "Dice"
                            },
                        new Resource
                            {
                                title = "RockPaperScissor"
                            },
                        new Resource
                            {
                                title = "Informing"
                            },
                        new Resource
                            {
                                title = "Contract"
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
            var KernelResource = await _context.resources.SingleAsync(exp => exp.title.Equals("Kernel"));
            var FinancialResource = await _context.resources.SingleAsync(exp => exp.title.Equals("Financial"));
            var BlockChainLogProcessorResource = await _context.resources.SingleAsync(exp => exp.title.Equals("BlockChainLogProcessor"));
            var BlockChainPaymentResource = await _context.resources.SingleAsync(exp => exp.title.Equals("BlockChainPayment"));
            var BlockChainResource = await _context.resources.SingleAsync(exp => exp.title.Equals("BlockChain"));
            var BlockChainTransferResource = await _context.resources.SingleAsync(exp => exp.title.Equals("BlockChainTransfer"));
            var OrderResource = await _context.resources.SingleAsync(exp => exp.title.Equals("Order"));
            var BasketResource = await _context.resources.SingleAsync(exp => exp.title.Equals("Basket"));
            var CatalogResource = await _context.resources.SingleAsync(exp => exp.title.Equals("Catalog"));
            var CustomerResource = await _context.resources.SingleAsync(exp => exp.title.Equals("Customer"));
            var CustomerClubResource = await _context.resources.SingleAsync(exp => exp.title.Equals("CustomerClub"));
            var CoinResource = await _context.resources.SingleAsync(exp => exp.title.Equals("Coin"));
            var DiceResource = await _context.resources.SingleAsync(exp => exp.title.Equals("Dice"));
            var RockPaperScissorResource = await _context.resources.SingleAsync(exp => exp.title.Equals("RockPaperScissor"));
            var InformingResource = await _context.resources.SingleAsync(exp => exp.title.Equals("Informing"));
            var ContractResource = await _context.resources.SingleAsync(exp => exp.title.Equals("Contract"));

            await _context.clients.AddRangeAsync(new List<Client>
            {
                new Client
                {
                    clientId = "adminPanelClientId",
                    clientSecret = "!AdminPanel@Tricksfor#2024",
                    tokenLifeTimeInSeconds = 36000,
                    enabled = true,
                    statement = "You are about to connect to Tricksfor Admin panel.",
                    uri = new Uri("https://admin.tricksfor.com"),
                    version = "1",
                    clientResources = new List<ClientResource>
                    {
                        new ClientResource
                        {
                            resource = BlockChainIdentityResource
                        },
                        new ClientResource
                        {
                            resource = KernelResource
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
                            resource = CustomerClubResource
                        },
                        new ClientResource
                        {
                            resource = CoinResource
                        },
                        new ClientResource
                        {
                            resource = DiceResource
                        },
                        new ClientResource
                        {
                            resource = RockPaperScissorResource
                        },
                        new ClientResource
                        {
                            resource = InformingResource
                        },
                        new ClientResource
                        {
                            resource = ContractResource
                        }
                    }
                },
                new Client
                {
                    clientId = "PwaAppClientId",
                    clientSecret = "(PWA)&(Tricksfor)*2024",
                    tokenLifeTimeInSeconds = 43200,
                    enabled = true,
                    statement = "You are about to connect to pwa.tricksfor.com.",
                    uri = new Uri("https://pwa.tricksfor.com"),
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
                            resource = CustomerClubResource
                        },
                        new ClientResource
                        {
                            resource = CoinResource
                        },
                        new ClientResource
                        {
                            resource = DiceResource
                        },
                        new ClientResource
                        {
                            resource = RockPaperScissorResource
                        },
                        new ClientResource
                        {
                            resource = InformingResource
                        },
                    }
                },
                new Client
                {
                    clientId = "WebAppClientId",
                    clientSecret = "(APP)&(Tricksfor)*2024",
                    tokenLifeTimeInSeconds = 43200,
                    enabled = true,
                    statement = "You are about to connect to app.tricksfor.com.",
                    uri = new Uri("https://app.tricksfor.com"),
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
                            resource = CustomerClubResource
                        },
                        new ClientResource
                        {
                            resource = CoinResource
                        },
                        new ClientResource
                        {
                            resource = DiceResource
                        },
                        new ClientResource
                        {
                            resource = RockPaperScissorResource
                        },
                        new ClientResource
                        {
                            resource = InformingResource
                        },
                    }
                },
                new Client
                {
                    clientId = "botClientId",
                    clientSecret = "botClientSecret",
                    tokenLifeTimeInSeconds = 300,
                    enabled = true,
                    statement = "You are about to connect to bot.tricksfor.com.",
                    uri = new Uri("https://bot.tricksfor.com"),
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
                            resource = CustomerClubResource
                        },
                        new ClientResource
                        {
                            resource = CoinResource
                        },
                        new ClientResource
                        {
                            resource = DiceResource
                        },
                        new ClientResource
                        {
                            resource = RockPaperScissorResource
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

    //private async Task TrySeedBaseParametersAsync()
    //{
    //    // Default data
    //    // Seed, if necessary
    //    if (!_context.baseParameters.Any())
    //    {
    //        await _context.baseParameters.AddRangeAsync(new List<BaseParameter>
    //        {
    //            new()
    //            {
    //                category = BaseParameterCategory.BlockChainIdentityConfiguration,
    //                field = BaseParameterField.BlockChainIdentityDefaultGeneratedSiweMessageLifeTime,
    //                value = "5"
    //            },
    //            new()
    //            {
    //                category = BaseParameterCategory.BlockChainIdentityConfiguration,
    //                field = BaseParameterField.BlockChainIdentitySecret,
    //                value = "CustomTokenValidationSecretKey123"
    //            },
    //            new()
    //            {
    //                category = BaseParameterCategory.BlockChainIdentityConfiguration,
    //                field = BaseParameterField.BlockChainPolygonMainNetChainId,
    //                value = "137"
    //            },
    //            new()
    //            {
    //                category = BaseParameterCategory.BlockChainIdentityConfiguration,
    //                field = BaseParameterField.BlockChainEthereumMainNetChainId,
    //                value = "1"
    //            },
    //        });
            
    //        await _context.SaveChangesAsync();
    //    }
    //}
}
