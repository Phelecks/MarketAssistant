using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Reflection;
using Asp.Versioning;
using BaseInfrastructure.Interceptors;
using BaseInfrastructure.Services;
using FluentValidation.AspNetCore;
using IdentityHelper;
using IdentityHelper.Helpers;
using Informing.Application;
using Informing.Grpc.Filters;
using Informing.Grpc.Handlers;
using Informing.Grpc.Hubs;
using Informing.Grpc.Services;
using Informing.Infrastructure;
using Informing.Infrastructure.Persistence;
using MassTransitManager.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Informing.Grpc;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        // builder.WebHost.ConfigureKestrel(options =>
        // {
        //     {
        //         var grpcPort = builder.Configuration.GetValue("GRPC_PORT", 80);
        //         options.Listen(IPAddress.Any, grpcPort, listenOptions =>
        //         {
        //             listenOptions.Protocols = HttpProtocols.Http2;
        //         });

        //         var apiRPort = builder.Configuration.GetValue("API_PORT", 81);
        //         if (apiRPort != 0)
        //             options.Listen(IPAddress.Any, apiRPort, listenOptions =>
        //             {
        //                 listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
        //             });
        //     }
        // });
        
        builder.AddRedisDistributedCache("cache");
        //builder.AddRabbitMQClient("messaging");

        services.AddSingleton<IIdentityHelper, Helpers.IdentityHelper>();

        builder.AddSqlServerClient(connectionName: "informingdb");
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
        //services.AddCodeFirstGrpc();

        services.AddMediatR(configuration: configuration =>
        {
            configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        });

        services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

        builder.Services.AddControllers(options =>
            options.Filters.Add<ApiExceptionFilter>());
        //builder.Services
        //    .AddFluentValidationAutoValidation(configurationExpression: x =>
        //    {
        //    })
        //    .AddFluentValidationClientsideAdapters(configuration: configuration =>
        //    {

        //    });
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
               options.ApplicationName = builder.Configuration.GetValue<string>("APPLICATION_NAME");
               options.ValidIssuers = new[] { builder.Configuration.GetValue<string>("BLOCKCHAIN_IDENTITY_AUTHORITY") };
               options.Events = new JwtBearerEvents
               {
                   OnMessageReceived = context =>
                   {
                       var accessToken = context.Request.Query["access_token"];

                       // If the request is for our hub...
                       var path = context.HttpContext.Request.Path;
                       if (!string.IsNullOrEmpty(accessToken) &&
                           (path.StartsWithSegments("/hubs")))
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

        // Initialise and seed database
        using var scope = app.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
        await initializer.InitialiseAsync();
        await initializer.SeedAsync();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<GreeterService>();
        //app.MapGrpcService<BaseparametersServiceV1>();
        //app.MapGrpcService<SchedulesServiceV1>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        if (app.Environment.IsDevelopment()) app.MapGrpcReflectionService();

        app.AddSignalRConfigurations();
        
        using var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope();
        var massTransitService = serviceScope?.ServiceProvider.GetRequiredService<IMassTransitService>();
        if(massTransitService is not null)
            await ResourceCreator.CreateResourceAsyc(massTransitService, app.Configuration.GetValue<string>("APPLICATION_NAME")!);
    }
}
