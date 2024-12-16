using Informing.Domain.Entities;
using LoggerService.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BaseInfrastructure.Common;

namespace Informing.Infrastructure.Persistence;

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
                eventId: EventTool.GetEventInformation(eventType: EventType.InformingException, eventName: "Initialize database"),
                exception: ex, message: "An error occurred while initializing the database"));
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
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
