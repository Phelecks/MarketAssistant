namespace Informing.Application.Interfaces;

public interface IGameHubProxy
{
    Task NotifyBetInitiatedAsync(string userId, Domain.SignalREntities.BetInitiatedDto dto);
    Task NotifyBetConfirmedAsync(string userId, Domain.SignalREntities.BetConfirmedDto dto);
    Task NotifyBetFailedAsync(string userId, Domain.SignalREntities.BetFailedDto dto);
    Task NotifyMatchOverviewUpdatedsync(Domain.SignalREntities.MatchOverviewDto dto);
    Task NotifyBetStatusAsync(string userId, Domain.SignalREntities.BetStatusDto dto);
}