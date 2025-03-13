using BaseInfrastructure;
using BlockChainIdentity.Infrastructure.MassTransit;
using BlockChainIdentity.Infrastructure.Persistence;
using BlockChainIdentity.Application.Interfaces;
using BlockChainIdentity.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BlockChainIdentity.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddBaseInfrastructureServices();

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var configService = serviceProvider.GetRequiredService<IConfiguration>();
            var useInMemoryDb = configService.GetValue("USE-INMEMORY-DATABASE", true);
            if(useInMemoryDb)
            {
                var databaseName = configService.GetValue<string>("ApplicationName");
                options.UseInMemoryDatabase(databaseName is null ? Guid.NewGuid().ToString() : databaseName)
                    .AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
            }
            else
            {
                var connectionString = configService.GetConnectionString("identitydb");
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

        //Add mass transit dependency injections
        services.AddMassTransitDependencyInjections();

        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}