using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Asp.Versioning;
using BaseInfrastructure.Interceptors;
using FluentValidation.AspNetCore;
using IdentityHelper;
using IdentityHelper.Helpers;
using Informing.Application;
using Informing.Grpc.Filters;
using Informing.Grpc.Handlers;
using Informing.Grpc.Hubs;
using Informing.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Informing.Grpc;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        builder.AddRedisDistributedCache("cache");
        builder.AddRabbitMQClient("messaging");

        services.AddSingleton<IIdentityHelper, Helpers.IdentityHelper>();

        var useInMemoryDb = builder.Configuration.GetValue("USE-INMEMORY-DATABASE", true);

        builder.AddSqlServerClient(connectionName: "informingdb", configureSettings: options =>
        {
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
               options.ValidIssuers = new[] { builder.Configuration.GetValue<string>("TOKEN-ISSUER")! };
               options.Events = new JwtBearerEvents
               {
                   OnMessageReceived = context =>
                   {
                       var accessToken = context.Request.Query["access_token"];

                       // If the request is for our hub...
                       var path = context.HttpContext.Request.Path;
                       if (!string.IsNullOrEmpty(accessToken) &&
                           path.StartsWithSegments("/hubs"))
                       {
                           // Read the token out of the query string
                           context.Token = accessToken;
                       }
                       return Task.CompletedTask;
                   }
               };
           });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options => { options.CustomSchemaIds(type => type.ToString());});

        builder.Services.AddIdentityHelperDependencyInjections();

        builder.Services.AddSignalRServices(builder.Configuration.GetConnectionString("redis"));

        return services;
    }

    public static async Task AddConfiguration(this WebApplication app)
    {
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
        app.MapGet("/", () => "Welcome to Informing application.");

        if (app.Environment.IsDevelopment()) app.MapGrpcReflectionService();

        app.AddSignalRConfigurations();

        app.MapDefaultEndpoints();

        await Task.CompletedTask;
    }
}
