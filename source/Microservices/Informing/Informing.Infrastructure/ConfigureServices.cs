using BaseInfrastructure;
using Discord.WebSocket;
using Informing.Application.Interfaces;
using Informing.Infrastructure.MassTransit;
using Informing.Infrastructure.Persistence;
using Informing.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IApplicationDbContext = Informing.Application.Interfaces.IApplicationDbContext;

namespace Informing.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddBaseInfrastructureServices();

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var configService = serviceProvider.GetRequiredService<IConfiguration>();
                var useInMemoryDb = configService.GetValue("USE_INMEMORY_DATABASE", true);
                if(useInMemoryDb)
                {
                    options.UseInMemoryDatabase(configService.GetValue<string>("ApplicationName", Guid.NewGuid().ToString()))
                        .AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
                }
                else
                {
                    var connectionString = configService.GetConnectionString("informingdb");
                    options.UseSqlServer(connectionString,
                        builder =>
                        {
                            builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                            builder.EnableRetryOnFailure();
                        })
                        .AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
                }
                
            });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitializer>();

        //Add communication dependency injections
        services.AddMassTransitDependencyInjections();

        services.AddScoped<IContactService, ContactService>();

        services.AddSingleton<DiscordSocketClient>();
        services.AddSingleton<IDiscordService, DiscordService>();

        return services;
    }
}
