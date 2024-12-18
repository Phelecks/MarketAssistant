using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using Ocelot.Provider.Polly;
using Ocelot.ServiceDiscovery;
using Admin.Gateway.Helpers;

namespace Admin.Gateway;

public static class ConfigureServices
{
    const string ProductCorsPoicyName = "ProductPolicy"; 
    static string[] ProductApplicationDomains = ["https://marketassitant.tricksfor.com"];
    static string[] StagingApplicationDomains = ["https://marketassitant-staging.tricksfor.com"];

    public static IServiceCollection AddServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();
        // builder.WebHost.ConfigureKestrel(options =>
        // {
        //     {
        //         var grpcPort = builder.Configuration.GetValue("GRPC_PORT", 0);
        //         if (grpcPort != 0)
        //             options.Listen(IPAddress.Any, grpcPort, listenOptions =>
        //             {
        //                 listenOptions.Protocols = HttpProtocols.Http2;
        //             });

        //         var apiRPort = builder.Configuration.GetValue("API_PORT", 81);
        //         if (apiRPort != 0)
        //             options.Listen(IPAddress.Any, apiRPort, listenOptions =>
        //             {
        //                 listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
        //             });
        //     }
        // });

        builder.Services.AddCors(options =>
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (!string.IsNullOrEmpty(environment) && environment.Equals("Development"))
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                            .AllowAnyMethod().AllowAnyHeader();
                    });
            else if (!string.IsNullOrEmpty(environment) && environment.Equals("Staging"))
                options.AddPolicy(ProductCorsPoicyName,
                    policy =>
                    {
                        policy.WithOrigins(StagingApplicationDomains)
                            .AllowAnyMethod().AllowAnyHeader();
                    });
            else
                options.AddPolicy(ProductCorsPoicyName,
                    policy =>
                    {
                        policy.WithOrigins(ProductApplicationDomains)
                            .AllowAnyMethod().AllowAnyHeader();
                    });
        });

        builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config
                    .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"configuration.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
            });

        builder.WebHost.UseIISIntegration();

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options => { options.CustomSchemaIds(type => type.ToString());});

        services.AddHttpContextAccessor();


        // builder.Services.AddAuthorization();
        // builder.Services.AddAuthentication();

        //ServiceDiscoveryFinderDelegate serviceDiscoveryFinder = (provider, serviceProviderConfiguration, downStreamRoute) =>
        //{
        //    var configuration = provider.GetRequiredService<IConfiguration>();
        //    return new AspireServiceDiscoveryProvider(downStreamRoute, configuration);
        //};
        services.AddSingleton<ServiceDiscoveryFinderDelegate>((provider, serviceProviderConfiguration, downStreamRoute) =>
            new AspireServiceDiscoveryProvider(downStreamRoute, provider.GetRequiredService<IConfiguration>()));
        builder.Services.AddOcelot()
            .AddCacheManager(settings: x => x.WithDictionaryHandle())
            //AddCustomServiceDiscovery
            //.AddConsul()
            //.AddConfigStoredInConsul()
            .AddPolly();

        return services;
    }

    public static async Task AddConfiguration(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //app.UseHsts();
        }

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (!string.IsNullOrEmpty(environment) && environment.Equals("Development"))
            app.UseCors();
        else
            app.UseCors(ProductCorsPoicyName);

        // app.UseAuthentication();
        // app.UseAuthorization();

        await app.UseOcelot();
    }
}
