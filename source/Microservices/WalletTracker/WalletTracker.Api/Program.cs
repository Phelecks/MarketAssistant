using WalletTracker.Api;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddServices(builder);

var app = builder.Build();

await app.AddConfiguration();

app.MapDefaultEndpoints();

app.Run();