using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface ICreateMatchWalletMessage : CorrelatedBy<Guid>
{
    string Game { get; }
    long MatchId { get; }
}