using Informing.Application.Interfaces;
using Informing.Domain.SignalREntities;
using Microsoft.AspNetCore.SignalR;

namespace Informing.Grpc.Hubs;

public class GameHubProxy : IGameHubProxy
{
    private readonly IHubContext<GameHub, IGameHub> _hubContext;

    public GameHubProxy(IHubContext<GameHub, IGameHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyBetInitiatedAsync(string userId, BetInitiatedDto dto)
    {
        await _hubContext.Clients.User(userId).BetInitiated(dto);
    }

    public async Task NotifyBetConfirmedAsync(string userId, BetConfirmedDto dto)
    {
        await _hubContext.Clients.User(userId).BetConfirmed(dto);
        await _hubContext.Clients.Group(dto.game).BetConfirmed(dto);
    }

    public async Task NotifyBetFailedAsync(string userId, BetFailedDto dto)
    {
        await _hubContext.Clients.User(userId).BetFailed(dto);
    }

    public async Task NotifyBetStatusAsync(string userId, BetStatusDto dto)
    {
        await _hubContext.Clients.User(userId).BetStatusChanged(dto);
    }

    public async Task NotifyMatchOverviewUpdatedsync(MatchOverviewDto dto)
    {
        await _hubContext.Clients.Group(dto.game).MatchOverviewUpdated(dto);
    }
}
