using BaseInfrastructure;
using BlockChainIdentity.Infrastructure.MassTransit;
using BlockChainIdentity.Infrastructure.Persistence;
using BlockChainIdentity.Application.Interfaces;
using BlockChainIdentity.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IApplicationDbContext = BlockChainIdentity.Application.Interfaces.IApplicationDbContext;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BlockChainIdentity.Infrastructure;

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

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitializer>();

        //Add mass transit dependency injections
        services.AddMassTransitDependencyInjections(configuration);

        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}