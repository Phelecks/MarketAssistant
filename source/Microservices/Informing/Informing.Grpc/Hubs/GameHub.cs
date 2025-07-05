using BaseApplication.Security;
using Informing.Application.Interfaces;
using LoggerService.Helpers;
using Microsoft.AspNetCore.SignalR;

namespace Informing.Grpc.Hubs;

[Authorize]
public class GameHub : Hub<IGameHub>
{
    private readonly ILogger<GameHub> _logger;

    public GameHub(ILogger<GameHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (exception is not null)
            _ = Task.Run(() => _logger.LogError(EventTool.GetEventInformation(EventType.Exception, "Hub disconnected"),
                exception, exception.Message, DateTimeOffset.Now));
        await base.OnDisconnectedAsync(exception);
    }

    public async Task AddToGameGroup(string game)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, game);
    }

    public async Task RemoveFromGameGroup(string game)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, game);
    }
}