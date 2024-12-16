using BlockChainIdentity.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder);

var app = builder.Build();

await app.AddConfiguration();

app.Run();

public partial class Program { }