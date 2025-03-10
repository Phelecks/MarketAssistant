﻿using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using IdentityHelper;
using BlockChainIdentity.Application;
using BlockChainIdentity.Infrastructure;
using BlockChainIdentity.Infrastructure.Persistence;
using BaseInfrastructure.Interceptors;
using BaseInfrastructure.Services;
using IdentityHelper.Helpers;
using BlockChainIdentity.Grpc.Filters;
using FluentValidation.AspNetCore;
using BlockChainIdentity.Grpc.Handlers;
using Asp.Versioning;
using MassTransit;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using MassTransitManager.Services;

namespace BlockChainIdentity.Grpc;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();
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

        builder.Services.Configure<Domain.ConfigurationOptions>(options =>
        {
            options.Issuer = builder.Configuration.GetValue<string>("TOKEN-ISSUER")!;
        });

        builder.AddRedisDistributedCache("cache");
        //builder.AddRabbitMQClient("messaging");

        services.AddSingleton<IIdentityHelper, Helpers.IdentityHelper>();

        var useInMemoryDb = builder.Configuration.GetValue("USE-INMEMORY-DATABASE", true);
        
        builder.AddSqlServerClient(connectionName: "identitydb", configureSettings: options =>
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
               options.ApplicationName = builder.Configuration.GetValue<string>("APPLICATION_NAME")!;
               options.ValidIssuers = new[] { builder.Configuration.GetValue<string>("TOKEN-ISSUER")! };
           });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options => { options.CustomSchemaIds(type => type.ToString());});

        builder.Services.AddIdentityHelperDependencyInjections();

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
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        if (app.Environment.IsDevelopment()) app.MapGrpcReflectionService();

        app.MapDefaultEndpoints();

        using var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope();
        var massTransitService = serviceScope?.ServiceProvider.GetRequiredService<IMassTransitService>();
        if(massTransitService is not null)
            await ResourceCreator.CreateResourceAsyc(massTransitService, app.Configuration.GetValue<string>("APPLICATION_NAME")!);
    }
}
