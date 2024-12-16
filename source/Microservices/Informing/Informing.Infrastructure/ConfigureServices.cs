using BaseApplication.Interfaces;
using BaseInfrastructure;
using BaseInfrastructure.Services;
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
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBaseInfrastructureServices();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                options.UseInMemoryDatabase(configuration.GetValue<string>("ApplicationName", Guid.NewGuid().ToString()))
                    .AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
            });
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(configuration.GetValue<string>("DEFAULT_CONNECTION_STRING"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                    .AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
            });
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<Persistence.ApplicationDbContext>());

        services.AddScoped<Persistence.ApplicationDbContextInitializer>();

        services.AddTransient<IDateTimeService, DateTimeService>();

        //Add communication dependency injections
        services.AddMassTransitDependencyInjections(configuration);

        services.AddScoped<IMailService, MailService>();
        services.AddScoped<IFCMService, FCMService>();
        services.AddScoped<IContactService, ContactService>();

        services.AddSingleton<DiscordSocketClient>();
        //services.AddSingleton<InteractionService>();
        services.AddSingleton<IDiscordService, DiscordService>();

        return services;
    }
}
