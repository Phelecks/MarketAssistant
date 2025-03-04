using LoggerService.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BaseInfrastructure.Common;

namespace BlockProcessor.Infrastructure.Persistence;

public class ApplicationDbContextInitializer : IApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
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
                eventId: EventTool.GetEventInformation(eventType: EventType.BlockProcessorException, eventName: "Initialize database"),
                exception: ex, message: "An error occurred while initializing the database"));
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedRpcUrlsAsync();
            await TrySeedWalletAddresesAsync();
            await TrySeedBlockProgressAsync();
        }
        catch (Exception ex)
        {
            _ = Task.Run(() => _logger.LogError(
                eventId: EventTool.GetEventInformation(eventType: EventType.BlockProcessorException, eventName: "Seed database"),
                exception: ex, message: "An error occurred while seeding the database."));
            throw;
        }
    }

    private async Task TrySeedRpcUrlsAsync()
    {
        if (!await _context.RpcUrls.AnyAsync())
        {
            var rpcUrls = new List<Domain.Entities.RpcUrl>
            {
                new() { Id = 1, Chain = Nethereum.Signer.Chain.Polygon, WaitIntervalOfBlockProgress = 10, BlockOfConfirmation = 3, Uri = new Uri("https://polygon-mainnet.g.alchemy.com/v2/22Jr03KTaxzY9R6szSsaYs2zumuPef9u"), }
            };

            await _context.RpcUrls.AddRangeAsync(rpcUrls);
            await _context.SaveChangesAsync();
        }
    }

    private async Task TrySeedWalletAddresesAsync()
    {
        if (!await _context.WalletAddresses.AnyAsync())
        {
            var walletAddresses = new List<Domain.Entities.WalletAddress>
            {
                new() { Address = "0x0B33Da9E1d07B72A8344948644eB5254919aa312" },
                new() { Address = "0x46b05Ea9031648E7F16F94e9876e73ab7d818633" },
                new() { Address = "0x429B8474BD7308b7787d364985bB4b8eA7De1D47" }
            };

            await _context.WalletAddresses.AddRangeAsync(walletAddresses);
            await _context.SaveChangesAsync();
        }
    }

    private async Task TrySeedBlockProgressAsync()
    {
        if (!await _context.BlockProgresses.AnyAsync())
        {
            var blockProgresses = new List<Domain.Entities.BlockProgress>
            {
                new() { Id = 1, Chain = Nethereum.Signer.Chain.Polygon, BlockNumber = 68654002, Status = Domain.Entities.BlockProgress.BlockProgressStatus.Processed}
            };

            await _context.BlockProgresses.AddRangeAsync(blockProgresses);
            await _context.SaveChangesAsync();
        }
    }
}
