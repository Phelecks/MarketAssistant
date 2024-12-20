using Informing.Domain.Entities;
using LoggerService.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BaseInfrastructure.Common;
using BaseDomain.Enums;
using Microsoft.Extensions.Configuration;

namespace Informing.Infrastructure.Persistence;

public class ApplicationDbContextInitializer : IApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger, ApplicationDbContext context, IConfiguration configuration)
    {
        _logger = logger;
        _context = context;
        _configuration = configuration;
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
                eventId: EventTool.GetEventInformation(eventType: EventType.InformingException, eventName: "Initialize database"),
                exception: ex, message: "An error occurred while initializing the database"));
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedBaseParametersAsync();
            await TrySeedTemplatesAsync();
            await TrySeedAdministratorsAsync();
        }
        catch (Exception ex)
        {
            _ = Task.Run(() => _logger.LogError(
                eventId: EventTool.GetEventInformation(eventType: EventType.InformingException, eventName: "Seed database"),
                exception: ex, message: "An error occurred while seeding the database."));
            throw;
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
                new()
                {
                    category = BaseParameterCategory.InformingConfiguration,
                    field = BaseParameterField.InformingDiscordBotToken,
                    value = _configuration.GetValue("DISCORD-BOT-TOKEN", "MyDiscordBotToken123")!
                }
            });

            await _context.SaveChangesAsync();
        }

        if (!await _context.baseParameters.AnyAsync(exp => exp.field == BaseParameterField.BlockChainTransferOrphanTransferThresholdInMinutes))
        {
            await _context.baseParameters.AddAsync(new()
            {
                category = BaseParameterCategory.BlockChainTransferConfiguration,
                field = BaseParameterField.BlockChainTransferOrphanTransferThresholdInMinutes,
                value = "1440"
            });
            await _context.SaveChangesAsync();
        }
    }


    private async Task TrySeedTemplatesAsync()
    {
        // Default data
        // Seed, if necessary
        if (!_context.templates.Any())
        {
            await _context.templates.AddRangeAsync(new List<Template>
            {
                new()
                {
                    content = "Verification Code From MYAPPLICATION: <b>@VerificationCode</b>",
                    title = "Verification Code",
                    informingSendType = BaseDomain.Enums.InformingEnums.InformingSendType.Email,
                    informingType = BaseDomain.Enums.InformingEnums.InformingType.VerifyEmailAddress
                },
                new()
                {
                    content = "An error occurred: </br> @Content",
                    title = "System Error",
                    informingSendType = BaseDomain.Enums.InformingEnums.InformingSendType.Email,
                    informingType = BaseDomain.Enums.InformingEnums.InformingType.SystemErrorMessage
                },
            });

            await _context.SaveChangesAsync();
        }
    }

    private async Task TrySeedAdministratorsAsync()
    {
        // Default data
        // Seed, if necessary
        if (!_context.contacts.Any(contact => contact.groupContacts.Any(groupContact => groupContact.group.title.Equals("Administrators"))))
        {
            await _context.contacts.AddRangeAsync(new List<Contact>
            {
                new()
                {
                    fullname = "Hamid Shayanian",
                    emailAddress = "hamid.shayanian@outlook.com",
                    phoneNumber = null,
                    userId = Guid.NewGuid().ToString(),
                    username = "hamid.shayanian@outlook.com",
                    groupContacts = new List<GroupContact> 
                    {
                        new GroupContact
                        {
                            group = new Group
                            {
                                title = "Administrators",
                                description = "System Administrators."
                            }
                        }
                    }
                },
            });

            await _context.SaveChangesAsync();
        }
    }
}
