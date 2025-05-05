using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Asp.Versioning;
using BaseInfrastructure.Interceptors;
using FluentValidation.AspNetCore;
using IdentityHelper;
using IdentityHelper.Helpers;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using BaseInfrastructure.Helpers;
using BaseApi.Extensions;
using BaseApi.Filters;
using BaseApi.Handlers;

namespace BaseApi;

public static class ConfigureServices
{
    public static IServiceCollection AddBaseApiServices(this IServiceCollection services, WebApplicationBuilder builder,
        string sqlConnectionName,
        string? redisDistributedCacheConnectionName, string? rabbitMQConnectionName)
        
    {
        builder.AddServiceDefaults();

        if(!string.IsNullOrEmpty(redisDistributedCacheConnectionName))builder.AddRedisDistributedCache(connectionName: redisDistributedCacheConnectionName);
        if(!string.IsNullOrEmpty(rabbitMQConnectionName))builder.AddRabbitMQClient(connectionName: rabbitMQConnectionName);

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

        builder.AddSqlServerClient(connectionName: sqlConnectionName, configureSettings: options =>
        {
            var useInMemoryDb = builder.Configuration.GetValue("USE-INMEMORY-DATABASE", true);
            if (useInMemoryDb) options.DisableHealthChecks = true;
        });

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

    public static void AddBaseApiConfiguration(this WebApplication app)
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
        app.MapGet("/", () => $"Welcome to {app.Configuration.GetValue("APPLICATION-NAME", "DefaultApplicationName")} application.");

        if (app.Environment.IsDevelopment()) app.MapGrpcReflectionService();

        app.MapDefaultEndpoints();
    }
}
