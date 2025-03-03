using BaseInfrastructure;
using BlockProcessor.Application.Interfaces;
using BlockProcessor.Infrastructure.MassTransit;
using BlockProcessor.Infrastructure.Persistence;
using BlockProcessor.Infrastructure.Services;
using DistributedProcessManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlockProcessor.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBaseInfrastructureServices();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var databaseName = configuration.GetValue<string>("ApplicationName");
                if(string.IsNullOrEmpty(databaseName)) databaseName = Guid.NewGuid().ToString();
                options.UseInMemoryDatabase(databaseName)
                    .AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
            });
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(configuration.GetValue<string>("blockprocessordb"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                    .AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
            });
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitializer>();

        //Add mass transit dependency injections
        services.AddMassTransitDependencyInjections(configuration);

        services.AddDistributedBlockProgressRepositoryDependencyInjections();
        services.AddSingleton<IBlockProcessorService, BlockProcessorService>();

        return services;
    }
}
