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
            //await TrySeedSmartContracsAsync();
        }
        catch (Exception ex)
        {
            _ = Task.Run(() => _logger.LogError(
                eventId: EventTool.GetEventInformation(eventType: EventType.BlockProcessorException, eventName: "Seed database"),
                exception: ex, message: "An error occurred while seeding the database."));
            throw;
        }
    }
}
