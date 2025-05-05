using BlockProcessor.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder);

var app = builder.Build();

app.AddConfiguration();

await app.RunAsync();

public partial class Program { }