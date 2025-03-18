using BaseInfrastructure;
using BlockChain.Application.Interfaces;
using BlockChain.Infrastructure.MassTransit;
using BlockChain.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlockChain.Infrastructure;

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
                var databaseName = configService.GetValue<string>("APPLICATION-NAME");
                options.UseInMemoryDatabase(databaseName is null ? Guid.NewGuid().ToString() : databaseName)
                    .AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
            }
            else
            {
                var connectionString = configService.GetConnectionString("blockchaindb");
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

        return services;
    }
}
