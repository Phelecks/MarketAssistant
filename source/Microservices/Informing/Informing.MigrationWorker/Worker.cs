using System.Diagnostics;
using BaseDomain.Enums;
using Informing.Domain.Entities;
using Informing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Informing.MigrationWorker;

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
            await TrySeedBaseParametersAsync(context, configuration, stoppingToken);
            await TrySeedTemplatesAsync(context, stoppingToken);
            await TrySeedAdministratorsAsync(context, stoppingToken);
            await context.SaveChangesAsync(stoppingToken);
            await transaction.CommitAsync(stoppingToken);
        });
    }

    private static async Task TrySeedBaseParametersAsync(ApplicationDbContext context, IConfiguration configuration, CancellationToken stoppingToken)
    {
        if (!await context.BaseParameters.AnyAsync(stoppingToken))
        {
            await context.BaseParameters.AddRangeAsync(new List<BaseParameter>
            {
                new()
                {
                    Category = BaseParameterCategory.InformingConfiguration,
                    Field = BaseParameterField.InformingDiscordBotToken,
                    Value = configuration.GetValue("DISCORD-BOT-TOKEN", "MyDiscordBotToken123")!
                }
            }, stoppingToken);

            await context.SaveChangesAsync();
        }

        if (!await context.BaseParameters.AnyAsync(exp => exp.Field == BaseParameterField.BlockChainTransferOrphanTransferThresholdInMinutes, cancellationToken: stoppingToken))
        {
            await context.BaseParameters.AddAsync(new()
            {
                Category = BaseParameterCategory.BlockChainTransferConfiguration,
                Field = BaseParameterField.BlockChainTransferOrphanTransferThresholdInMinutes,
                Value = "1440"
            });
            await context.SaveChangesAsync(stoppingToken);
        }
    }
    private static async Task TrySeedTemplatesAsync(ApplicationDbContext context, CancellationToken stoppingToken)
    {
        if (!await context.Templates.AnyAsync(stoppingToken))
        {
            await context.Templates.AddRangeAsync(new List<Template>
            {
                new()
                {
                    Content = "Verification Code From MYAPPLICATION: <b>@VerificationCode</b>",
                    Title = "Verification Code",
                    InformingSendType = InformingEnums.InformingSendType.Email,
                    InformingType = InformingEnums.InformingType.VerifyEmailAddress
                },
                new()
                {
                    Content = "An error occurred: </br> @Content",
                    Title = "System Error",
                    InformingSendType = InformingEnums.InformingSendType.Email,
                    InformingType = InformingEnums.InformingType.SystemErrorMessage
                },
            }, stoppingToken);

            await context.SaveChangesAsync(stoppingToken);
        }
    }
    private static async Task TrySeedAdministratorsAsync(ApplicationDbContext context, CancellationToken stoppingToken)
    {
        if (!await context.Contacts.AnyAsync(contact => contact.GroupContacts.Any(groupContact => groupContact.Group.Title.Equals("Administrators")), stoppingToken))
        {
            await context.Contacts.AddRangeAsync(new List<Contact>
            {
                new()
                {
                    Fullname = "Hamid Shayanian",
                    EmailAddress = "hamid.shayanian@outlook.com",
                    PhoneNumber = null,
                    UserId = Guid.NewGuid().ToString(),
                    Username = "hamid.shayanian@outlook.com",
                    GroupContacts = new List<GroupContact> 
                    {
                        new GroupContact
                        {
                            Group = new Group
                            {
                                Title = "Administrators",
                                Description = "System Administrators."
                            }
                        }
                    }
                },
            }, stoppingToken);

            await context.SaveChangesAsync(stoppingToken);
        }
    }
}
