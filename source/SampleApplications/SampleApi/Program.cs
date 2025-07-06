using CustomMediatR.Services;
using CustomMediatR;
using SampleApi.Ping.Commands.PingWithResponse;
using SampleApi.Ping.Commands.PingWithoutResponse;
using SampleApi.Ping.NotificationHandlers.PingCreated;
using CustomMediatR.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomMediator();

var app = builder.Build();

// app.MapGet("/", () => "Hello World.");

app.MapGet("/", async (IRequestDistpacher dispatcher, CancellationToken cancellationToken) =>
{
    var command = new PingWithResponseCommand("Sample content");
    var result = await dispatcher.SendAsync(command, cancellationToken);
    await dispatcher.PublishAsync(new PingCreated("Sample Content"), cancellationToken);
    return Results.Ok(result);
});

app.MapGet("/withoutresponse", async (IRequestDistpacher dispatcher, CancellationToken cancellationToken) =>
{
    var command = new PingWithoutResponseCommand("Sample content");
    var result = await dispatcher.SendAsync(command, cancellationToken);
    return Results.Ok();
});

app.MapGet("/ValidateError", async (IRequestDistpacher dispatcher, CancellationToken cancellationToken) =>
{
    var command = new PingWithResponseCommand(null);
    var result = await dispatcher.SendAsync(command, cancellationToken);
    return Results.Ok(result);
});

app.Run();
