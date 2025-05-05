using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

const string ProductCorsPoicyName = "ProductPolicy";
string[] ProductApplicationDomains = ["https://marketasssitant.tricksfor.com"];
string[] StagingApplicationDomains = ["https://marketasssitant-staging.tricksfor.com"];

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

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

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixPolicy", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromSeconds(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGet("/", () => "Hello World!");

app.UseRateLimiter();

app.MapReverseProxy(proxyPipeline =>
{
    /// proxyPipeline.UseSessionAffinity();

    /// proxyPipeline.UseLoadBalancing();
});

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
if (!string.IsNullOrEmpty(environment) && environment.Equals("Development"))
    app.UseCors();
else
    app.UseCors(ProductCorsPoicyName);

await app.RunAsync();
