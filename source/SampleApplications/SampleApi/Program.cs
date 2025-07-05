using CustomMediatR.Services;
using SampleApi.Ping.Commands;
using CustomMediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomMediator();

var app = builder.Build();

// app.MapGet("/", () => "Hello World.");

app.MapGet("/", async (RequestDispatcher dispatcher, CancellationToken cancellationToken) =>
{
    var command = new PingCommand("Sample content");
    var result = await dispatcher.SendAsync(command, cancellationToken);
    return Results.Ok(result);
});


app.MapGet("/ValidateError", async (RequestDispatcher dispatcher, CancellationToken cancellationToken) =>
{
    var command = new PingCommand(null);
    var result = await dispatcher.SendAsync(command, cancellationToken);
    return Results.Ok(result);
});

app.Run();
