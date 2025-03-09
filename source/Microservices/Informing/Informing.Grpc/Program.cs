using Informing.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder);

var app = builder.Build();

await app.AddConfiguration();

await app.RunAsync();

public partial class Program { }