using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Asp.Versioning;
using BaseInfrastructure.Interceptors;
using FluentValidation.AspNetCore;
using IdentityHelper;
using IdentityHelper.Helpers;
using BlockChain.Application;
using BlockChain.Infrastructure;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using BaseInfrastructure.Helpers;
using BlockChain.Api.Extensions;
using BlockChain.Api.Filters;
using BlockChain.Api.Handlers;

namespace BlockChain.Api;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        builder.AddRedisDistributedCache("cache");
        builder.AddRabbitMQClient(connectionName: "messaging");

        builder.Services.AddOutputCache(options =>
        {
            options.AddBasePolicy(builder =>
                builder.Expire(TimeSpan.FromSeconds(5)));
            options.AddPolicy(OutputCacheKeys.CacheForTenSeconds, builder =>
                builder.Expire(TimeSpan.FromSeconds(10)));
            options.AddPolicy(OutputCacheKeys.CacheForThirtySeconds, builder =>
                builder.Expire(TimeSpan.FromSeconds(30)));
            options.AddPolicy(OutputCacheKeys.CacheForOneMinute, builder =>
                builder.Expire(TimeSpan.FromSeconds(60)));
        });

        services.AddSingleton<IIdentityHelper, Helpers.IdentityHelper>();

        builder.AddSqlServerClient(connectionName: "blockchaindb", configureSettings: options =>
        {
            var useInMemoryDb = builder.Configuration.GetValue("USE-INMEMORY-DATABASE", true);
            if (useInMemoryDb) options.DisableHealthChecks = true;
        });
        services.AddInfrastructureServices();
        services.AddApplicationServices();

        services.AddDatabaseDeveloperPageExceptionFilter();
        
        services.AddHttpContextAccessor();

        services.AddGrpcReflection();
        services.AddGrpc(options =>
        {
            options.ResponseCompressionAlgorithm = "gzip";
            options.Interceptors.Add<GrpcGlobalExceptionHandlerInterceptor>();
            options.EnableDetailedErrors = true;
        });

        services.AddMediatR(configuration: configuration =>
        {
            configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        });

        builder.Services.AddControllers(options =>
            options.Filters.Add<ApiExceptionFilter>());
            
        builder.Services.AddFluentValidationAutoValidation(configuration =>
        {
            configuration.DisableBuiltInModelValidation = true;
            configuration.EnableFormBindingSourceAutomaticValidation = false;
            configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
        });

        builder.Services.AddApiVersioning(options =>
        {
            // reporting api versions will return the headers
            // "api-supported-versions" and "api-deprecated-versions"
            options.ReportApiVersions = true;
            options.DefaultApiVersion = new ApiVersion(majorVersion: 1, minorVersion: 0);
            options.ApiVersionReader = ApiVersionReader.Combine(
                new HeaderApiVersionReader()
                {
                    HeaderNames = { "x-api-version" }
                });
        });

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(SiweAuthenticationOptions.DefaultScheme)
           .AddScheme<SiweAuthenticationOptions, SiweAuthenticationHandler>(SiweAuthenticationOptions.DefaultScheme, options =>
           {
               options.ApplicationName = builder.Configuration.GetValue<string>("APPLICATION-NAME")!;
               options.ValidIssuers = [builder.Configuration.GetValue<string>("TOKEN-ISSUER")!];
           });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options => { options.CustomSchemaIds(type => type.ToString());});

        builder.Services.AddIdentityHelperDependencyInjections();

        return services;
    }

    public static async Task AddConfiguration(this WebApplication app)
    {
        //Map minimal APIs
        app.RegisterEndpointDefinitions();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();

            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        // Configure the HTTP request pipeline.
        app.MapGet("/", () => "Welcome to BlockChain application.");

        if (app.Environment.IsDevelopment()) app.MapGrpcReflectionService();

        app.MapDefaultEndpoints();

        await Task.CompletedTask;
    }
}
