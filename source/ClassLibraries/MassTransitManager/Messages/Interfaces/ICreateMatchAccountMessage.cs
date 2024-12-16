using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface ICreateMatchAccountMessage : CorrelatedBy<Guid>
{
    string Game { get; }
    long MatchId { get; }
    string WalletAddress { get; }
}