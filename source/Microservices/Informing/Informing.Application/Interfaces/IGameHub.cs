namespace Informing.Application.Interfaces;

public interface IGameHub
{
    Task BetInitiated(Domain.SignalREntities.BetInitiatedDto dto);
    Task BetConfirmed(Domain.SignalREntities.BetConfirmedDto dto);
    Task BetFailed(Domain.SignalREntities.BetFailedDto dto);
    Task BetStatusChanged(Domain.SignalREntities.BetStatusDto dto);
    Task MatchOverviewUpdated(Domain.SignalREntities.MatchOverviewDto dto);
}
