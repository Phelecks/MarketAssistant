using WalletTracker.Api;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddServices(builder);

var app = builder.Build();

app.AddConfiguration();

await app.RunAsync();