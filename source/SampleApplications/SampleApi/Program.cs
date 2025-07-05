using CustomMediatR.Services;
using SampleApi.Ping.Commands;
using CustomMediatR;
using SampleApi.Ping.Commands.PingWithResponse;
using SampleApi.Ping.Commands.PingWithoutResponse;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomMediator();

var app = builder.Build();

// app.MapGet("/", () => "Hello World.");

app.MapGet("/", async (RequestDispatcher dispatcher, CancellationToken cancellationToken) =>
{
    var command = new PingWithResponseCommand("Sample content");
    var result = await dispatcher.SendAsync(command, cancellationToken);
    return Results.Ok(result);
});

app.MapGet("/withoutresponse", async (RequestDispatcher dispatcher, CancellationToken cancellationToken) =>
{
    var command = new PingWithoutResponseCommand("Sample content");
    var result = await dispatcher.SendAsync(command, cancellationToken);
    return Results.Ok();
});

app.MapGet("/ValidateError", async (RequestDispatcher dispatcher, CancellationToken cancellationToken) =>
{
    var command = new PingWithResponseCommand(null);
    var result = await dispatcher.SendAsync(command, cancellationToken);
    return Results.Ok(result);
});

app.Run();
